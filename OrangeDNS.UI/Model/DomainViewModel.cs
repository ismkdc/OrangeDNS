using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrangeDNS.UI.Model
{
    public class DomainViewModel
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string ForwardIp { get; set; }
        public string Type { get; set; }
        public string Category { get; set; }
    }
}