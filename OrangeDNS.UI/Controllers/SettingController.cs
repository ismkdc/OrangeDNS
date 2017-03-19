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
    public class SettingController : Controller
    {
        // GET: Setting
        public ActionResult Index()
        {
            if (Session["UserId"] == null) return RedirectToAction("login", "home");

            List<GeneralSettings> settings = null;
            using (OrangeDNSDataContext dc = new OrangeDNSDataContext())
            {
                settings = dc.GeneralSettings.ToList();
            }
            return View(settings);
        }
        public ActionResult Edit(int SettingId)
        {
            if (Session["UserId"] == null) return RedirectToAction("login", "home");
            using (OrangeDNSDataContext dc = new OrangeDNSDataContext())
            {
                GeneralSettings setting = dc.GeneralSettings.SingleOrDefault(s => s.Id == SettingId);
                return View(setting);
            }
        }
        [HttpPost]
        public ActionResult Edit(GeneralSettings Model)
        {
            if (Session["UserId"] == null) return RedirectToAction("login", "home");
            using (OrangeDNSDataContext dc = new OrangeDNSDataContext())
            {
                GeneralSettings setting = dc.GeneralSettings.SingleOrDefault(s => s.Name == Model.Name);
                setting.Value = Model.Value;
                dc.SaveChanges();
                ViewData["ResultMsg"] = "Değer başarıyla güncellendi";
                return View(setting);
            }
        }

    }
}