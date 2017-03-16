using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrangeDNS.UI.Controllers
{
    public class LogController : Controller
    {
        // GET: Log


        public ActionResult Index(string data, string type = "default")
        {
            if (Session["UserId"] == null) return RedirectToAction("login", "home");
            List<OrangeDNS.Data.Log> logs = null;
            using (OrangeDNS.Data.OrangeDNSDataContext dc = new Data.OrangeDNSDataContext())
            {
                switch (type)
                {
                    case "date":
                        DateTime date = DateTime.Parse(data);
                        logs = dc.Logs.Where(l => EntityFunctions.TruncateTime(l.Date) == EntityFunctions.TruncateTime(date)).ToList();
                        break;
                    case "ip":
                        logs = dc.Logs.Where(l => l.ClientIp == data).ToList();
                        break;
                    case "request":
                        logs = dc.Logs.Where(l => l.Request == data).ToList();
                        break;
                    case "result":
                        logs = dc.Logs.Where(l => l.Result.ToString() == data).ToList();
                        break;
                    default:
                        logs = dc.Logs.ToList();
                        break;
                }

            }
            return View(logs);
        }
    }
}