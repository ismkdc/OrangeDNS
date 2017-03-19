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