using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OrangeDNS.UI.Model;
using OrangeDNS.Data;
using System.Security.Cryptography;
using System.Text;

namespace OrangeDNS.UI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Session["UserId"] == null) return RedirectToAction("login");
            using (OrangeDNSDataContext dc = new OrangeDNSDataContext())
            {
                List<Model.TopDomainsViewModel> topDomains = dc.Logs.GroupBy(l => l.Request).Select(d => new Model.TopDomainsViewModel() { Name = d.Key, Count = d.Count() }).OrderByDescending(d => d.Count).Take(5).ToList();
                int counter = 1;
                foreach (var item in topDomains)
                {
                    item.Order = counter;
                    counter++;
                }
                ViewData["TopDomains"] = topDomains;

                List<Model.TopUsersViewModel> topUsers = dc.Logs.GroupBy(l => l.ClientIp).Select(d => new Model.TopUsersViewModel() { Ip = d.Key, Count = d.Count() }).OrderByDescending(d => d.Count).Take(5).ToList();
                 counter = 1;
                foreach (var item in topUsers)
                {
                    item.Name = dc.Clients.SingleOrDefault(c => c.Ip == item.Ip).Name;
                    item.Order = counter;
                    counter++;
                }
                ViewBag.Internet = dc.GeneralSettings.SingleOrDefault(s => s.Name == "GlobalBlock").Value;
                ViewData["TopUsers"] = topUsers;
            }
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult LogOut()
        {
            Session["UserId"] = null;
            return View("Login");
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (OrangeDNSDataContext dc = new OrangeDNSDataContext())
                {
                    model.Password = getSHA1Hash(model.Password);
                    OrangeDNS.Data.User user = dc.Users.SingleOrDefault(u => u.Username == model.Username && u.Password == model.Password);
                    if (user != null)
                    {
                        Session["UserId"] = user.Id;
                        Session["name"] = user.Name;
                        return RedirectToAction("index");
                    }
                    else
                    {
                        ViewBag.ErrorMsg = "Kullanıcı adı veya şifre hatalı";
                    }
                }
            }
            return View();
        }
        public string getSHA1Hash(string input)

        {

            SHA1 sha1Hasher = SHA1.Create();

            byte[] data = sha1Hasher.ComputeHash(Encoding.Default.GetBytes(input));

            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)

            {

                sBuilder.Append(data[i].ToString("x2"));

            }

            return sBuilder.ToString();

        }
        [HttpPost]
        public string Internet()
        {
            if (Session["UserId"] == null) return "";
            using (OrangeDNSDataContext dc = new OrangeDNSDataContext())
            {
                GeneralSettings setting = dc.GeneralSettings.SingleOrDefault(s => s.Name == "GlobalBlock");
                setting.Value = setting.Value == "1" ? "0" : "1";
                dc.SaveChanges();
            }
            return "1";
        }
    }
}