#region copyright
// Copyright (c) 2017 All Rights Reserved
// Author: İsmail Kundakcı
// Author Website: www.ismailkundakci.com
// Author Email: ism.kundakci@hotmail.com
// Date: 19/03/2017 13:15:00
// Description: OrangeDNS is a powerfull dns firewall solution written by C# used ARSoft.Tools.Net library
// ARSoft.Tools.Net Page: arsofttoolsnet.codeplex.com
#endregion
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrangeDNS.Data
{
   public class Client
    {
        [Key]
        public string Ip { get; set; }
        public string Name { get; set; }
        public bool IsBlocked { get; set; }
        public virtual ICollection<Log> Logs { get; set; }
        public Client()
        {
            Logs = new List<Data.Log>();
        }
    }
}
