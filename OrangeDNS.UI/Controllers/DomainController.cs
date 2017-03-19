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
using OrangeDNS.UI.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrangeDNS.UI.Controllers
{
    public class DomainController : Controller
    {
        // GET: Domain
        public ActionResult Index()
        {
            if (Session["UserId"] == null) return RedirectToAction("login", "home");
            using (OrangeDNSDataContext dc = new OrangeDNSDataContext())
            {
                ViewData["Domains"] = dc.Domains.Select(d => new DomainViewModel() { Id = d.Id, Url = d.Url, ForwardIp = d.ForwardIp, Type = d.Type.ToString(), Category = d.Category.Name }).ToList();
            }
            return View();
        }
        public ActionResult Add()
        {
            if (Session["UserId"] == null) return RedirectToAction("login", "home");
            using (OrangeDNSDataContext dc = new OrangeDNSDataContext())
            {
                ViewData["Categories"] = dc.Categories.ToList();
            }
            return View();
        }
        [HttpPost]
        public ActionResult Add(Data.Domain Model)
        {
            if (Session["UserId"] == null) return RedirectToAction("login", "home");
            using (OrangeDNSDataContext dc = new OrangeDNSDataContext())
            {
                ViewData["Categories"] = dc.Categories.ToList();
                if (!ModelState.IsValid)
                { ViewData["ResultMsg"] = "Hata domain eklenemedi!"; return View(); }

                dc.Domains.Add(Model);
                dc.SaveChanges();
                ModelState.Clear();
                ViewData["ResultMsg"] = "Domain başarıyla eklendi!";
            }
            return View();
        }
        public ActionResult BulkAdd()
        {
            if (Session["UserId"] == null) return RedirectToAction("login", "home");
            using (OrangeDNSDataContext dc = new OrangeDNSDataContext())
            {
                ViewData["Categories"] = dc.Categories.ToList();
            }
            return View();
        }
        [HttpPost]
        public ActionResult BulkAdd(HttpPostedFileBase myFile, int type, string forwardIp, int category)
        {
            if (Session["UserId"] == null) return RedirectToAction("login", "home");
            using (OrangeDNSDataContext dc = new OrangeDNSDataContext())
            {
                ViewData["Categories"] = dc.Categories.ToList();
                BinaryReader b = new BinaryReader(myFile.InputStream);
                byte[] binData = b.ReadBytes((int)myFile.InputStream.Length);
                string result = System.Text.Encoding.UTF8.GetString(binData);
                string[] domains = result.Split('\n');
                foreach (var item in domains)
                {
                    dc.Domains.Add(new Domain() { Url = item, Type = (DType)type, CategoryId = category, ForwardIp = forwardIp });
                }
                dc.SaveChanges();
            }
            ViewData["ResultMsg"] = "Domainler başarıyla eklendi!";
            return View();
        }
        public ActionResult Delete(int domainId)
        {
            if (Session["UserId"] == null) return RedirectToAction("login", "home");
            using (OrangeDNSDataContext dc = new OrangeDNSDataContext())
            {
                Domain domain = dc.Domains.SingleOrDefault(d => d.Id == domainId);
                dc.Domains.Remove(domain);
                dc.SaveChanges();
                ViewData["Domains"] = dc.Domains.Select(d => new DomainViewModel() { Id = d.Id, Url = d.Url, ForwardIp = d.ForwardIp, Type = d.Type.ToString(), Category = d.Category.Name }).ToList();
            }
            ViewData["ResultMsg"] = "Domain başarıyla silindi!";
            return View("index");
        }
    }
}