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