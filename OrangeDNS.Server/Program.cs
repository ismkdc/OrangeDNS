#region copyright
// Copyright (c) 2017 All Rights Reserved
// Author: İsmail Kundakcı
// Author Website: www.ismailkundakci.com
// Author Email: ism.kundakci@hotmail.com
// Date: 19/03/2017 13:15:00
// Description: OrangeDNS is a powerfull dns firewall solution written by C# used ARSoft.Tools.Net library
// ARSoft.Tools.Net Page: arsofttoolsnet.codeplex.com
#endregion
using ARSoft.Tools.Net;
using ARSoft.Tools.Net.Dns;
using OrangeDNS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OrangeDNS.Server
{
    class Program
    {
        static List<Domain> domainList;
        static OrangeDNSDataContext dc;
        static int updateTime;
        static string mainDns;
        static string globalBlock;
        static string globalBlockIp;
        static void Main(string[] args)
        {
            dc = new OrangeDNSDataContext();
            updateTime = int.Parse(dc.GeneralSettings.SingleOrDefault(s => s.Name == "UpdateTime").Value);
            mainDns = dc.GeneralSettings.SingleOrDefault(s => s.Name == "MainDns").Value;
            Timer updateData = new Timer(TimerCallback, null, 0, updateTime);
            using (DnsServer server = new DnsServer(System.Net.IPAddress.Any, 10, 10))
            {
                server.QueryReceived += OnQueryReceived;
                server.Start();
                while (true)
                {
                    string input = Console.ReadLine();
                    switch (input)
                    {
                        case "exit":
                            return;
                        case "update":
                            TimerCallback(null);
                            break;
                        default:
                            break;
                    }
                }

            }


        }

        private static void TimerCallback(Object o)
        {
            try
            {
                domainList = dc.Domains.ToList();
                globalBlock = dc.GeneralSettings.SingleOrDefault(s => s.Name == "GlobalBlock").Value;
                globalBlockIp = dc.GeneralSettings.SingleOrDefault(s => s.Name == "GlobalBlockIp").Value;
                mainDns = dc.GeneralSettings.SingleOrDefault(s => s.Name == "MainDns").Value;
                dc.Dispose();
                dc = new OrangeDNSDataContext();
            }
            catch (Exception e) { Console.WriteLine(e.Message); }

            Console.WriteLine("Domain list updated: " + DateTime.Now);
            GC.Collect();
        }
        static async Task OnQueryReceived(object sender, QueryReceivedEventArgs e)
        {
            DnsMessage message = e.Query as DnsMessage;
            DnsMessage query = e.Query as DnsMessage;

            if (query == null)
                return;

            DnsMessage response = query.CreateResponseInstance();
            if (e.RemoteEndpoint.Address == null) return;
            string ipAdr = e.RemoteEndpoint.Address.ToString();
            Client client = dc.Clients.SingleOrDefault(c => c.Ip == ipAdr);
            if(client == null)
            {
                dc.Clients.Add(new Client() { Ip = ipAdr });
                dc.SaveChanges();
            }
       

            if ((message.Questions.Count == 1))
            {
                DnsQuestion question = message.Questions[0];
                if (client.IsBlocked)
                {
                    dc.Logs.Add(new Log() { ClientIp = ipAdr, Request = question.Name.ToString(), Result = Log.ResultType.success });
                    dc.SaveChanges();
                    return;
                } 
                DnsClient dnsClient = new DnsClient(IPAddress.Parse(mainDns), 5000);
                DnsMessage upstreamResponse = await dnsClient.ResolveAsync(question.Name, question.RecordType, question.RecordClass);
                if (globalBlock == "1")
                {
                    response.AnswerRecords.AddRange(
                       upstreamResponse.AnswerRecords
                           .Where(w => !(w is ARecord))
                           .Concat(
                               upstreamResponse.AnswerRecords
                                   .OfType<ARecord>()
                                   .Select(a => new ARecord(a.Name, a.TimeToLive, IPAddress.Parse(globalBlockIp)))
                           )
                   );
                    dc.Logs.Add(new Log() { ClientIp = ipAdr, Request = question.Name.ToString(), Result = Log.ResultType.blocked });
                    dc.SaveChanges();
                }
                else
                {
                    string questionUrl = question.Name.ToString();
                    Domain questionDomain = domainList.SingleOrDefault(d => questionUrl.Contains(d.Url));
                    if (questionDomain == null)
                    {
                        if (upstreamResponse.AnswerRecords[0] != null)
                        {
                            foreach (DnsRecordBase record in (upstreamResponse.AnswerRecords))
                            {
                                response.AnswerRecords.Add(record);
                            }
                            foreach (DnsRecordBase record in (upstreamResponse.AdditionalRecords))
                            {
                                response.AdditionalRecords.Add(record);
                            }

                            response.ReturnCode = ReturnCode.NoError;
                            dc.Logs.Add(new Log() { ClientIp = ipAdr, Request = question.Name.ToString(), Result = Log.ResultType.success });
                            dc.SaveChanges();
                        }
                    }

                    else
                    {
                        if (questionDomain.Type == Data.DType.blocked)
                        {
                            response.AnswerRecords.AddRange(
                            upstreamResponse.AnswerRecords
                                .Where(w => !(w is ARecord))
                                .Concat(
                                    upstreamResponse.AnswerRecords
                                        .OfType<ARecord>()
                                        .Select(a => new ARecord(a.Name, a.TimeToLive, IPAddress.Parse(globalBlockIp)))
                                )
                        );
                            dc.Logs.Add(new Log() { ClientIp = ipAdr, Request = question.Name.ToString(), Result = Log.ResultType.blocked });
                            dc.SaveChanges();
                        }
                        else
                        {
                            response.AnswerRecords.AddRange(
                                                    upstreamResponse.AnswerRecords
                                                        .Where(w => !(w is ARecord))
                                                        .Concat(
                                                            upstreamResponse.AnswerRecords
                                                                .OfType<ARecord>()
                                                                .Select(a => new ARecord(a.Name, a.TimeToLive, IPAddress.Parse(questionDomain.ForwardIp)))
                                                        )
                                                );
                            dc.Logs.Add(new Log() { ClientIp = ipAdr, Request = question.Name.ToString(), Result = Log.ResultType.forward });
                            dc.SaveChanges();
                        }

                    }
                }

                e.Response = response;
            }
        }


    }
}
