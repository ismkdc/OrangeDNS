using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrangeDNS.UI.Model
{
    public class TopUsersViewModel
    {
        public string Name { get; set; }
        public string Ip { get; set; }
        public int Count { get; set; }
        public int Order { get; set; }
    }
}