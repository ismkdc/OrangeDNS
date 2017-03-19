#region copyright
// Copyright (c) 2017 All Rights Reserved
// Author: İsmail Kundakcı
// Author Website: www.ismailkundakci.com
// Author Email: ism.kundakci@hotmail.com
// Date: 19/03/2017 13:15:00
// Description: OrangeDNS is a powerfull dns firewall solution written by C# used ARSoft.Tools.Net library
// ARSoft.Tools.Net Page: arsofttoolsnet.codeplex.com
#endregion
using OrangeDNS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrangeDNS.UI.Controllers
{
    public class ClientController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            if (Session["UserId"] == null) return RedirectToAction("login", "home");
            List<Client> clients = null;
            using (OrangeDNSDataContext dc = new OrangeDNSDataContext())
            {
                clients = dc.Clients.ToList();
            }
            return View(clients);
        }
        public ActionResult Delete(string clientIp)
        {
            if (Session["UserId"] == null) return RedirectToAction("login", "home");
            List<Client> clients = null;
            using (OrangeDNSDataContext dc = new OrangeDNSDataContext())
            {
                Client client = dc.Clients.SingleOrDefault(d => d.Ip == clientIp);
                foreach (var item in dc.Logs.Where(l => l.ClientIp == clientIp))
                {
                    dc.Logs.Remove(item);
                }
                dc.Clients.Remove(client);
                dc.SaveChanges();
                clients = dc.Clients.ToList();
            }
            ViewData["ResultMsg"] = "Kullanıcı başarıyla silindi!";
            return View("index", clients);
        }
        public ActionResult Block(string clientIp)
        {
            if (Session["UserId"] == null) return RedirectToAction("login", "home");
            List<Client> clients = null;
            using (OrangeDNSDataContext dc = new OrangeDNSDataContext())
            {
                Client client = dc.Clients.SingleOrDefault(d => d.Ip == clientIp);
                if (client.IsBlocked)
                {
                    client.IsBlocked = false;
                    ViewData["ResultMsg"] = "Kullanıcı bloğu başarıyla kaldırıldı!";
                }
                else
                {
                    client.IsBlocked = true;
                    ViewData["ResultMsg"] = "Kullanıcı başarıyla bloklandı!";
                }
                dc.SaveChanges();
                clients = dc.Clients.ToList();
            }

            return View("index", clients);
        }

        public ActionResult Add()
        {
            if (Session["UserId"] == null) return RedirectToAction("login", "home");

            return View();
        }
        [HttpPost]
        public ActionResult Add(Client Model)
        {
            if (Session["UserId"] == null) return RedirectToAction("login", "home");
            using (OrangeDNSDataContext dc = new OrangeDNSDataContext())
            {
                if (!ModelState.IsValid)
                { ViewData["ResultMsg"] = "Hata kullanıcı eklenemedi!"; return View(); }

                Client client = dc.Clients.SingleOrDefault(c => c.Ip == Model.Ip);
                if (client != null)
                {
                    client.Name = Model.Name;
                }
                else
                {
                    dc.Clients.Add(Model);
                }

                dc.SaveChanges();
                ModelState.Clear();
                ViewData["ResultMsg"] = "Kullanıcı başarıyla eklendi!";
            }
            return View();
        }
        public JsonResult Search()
        {
            if (Session["UserId"] == null) return Json("");

            using (OrangeDNSDataContext dc = new OrangeDNSDataContext())
            {
                return Json(dc.Clients.Where(l => String.IsNullOrEmpty(l.Name)).Select(l => l.Ip).ToList(), JsonRequestBehavior.AllowGet);
            }
        }

    }
}