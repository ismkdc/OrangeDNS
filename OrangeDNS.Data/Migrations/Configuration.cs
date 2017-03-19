#region copyright
// Copyright (c) 2017 All Rights Reserved
// Author: İsmail Kundakcı
// Author Website: www.ismailkundakci.com
// Author Email: ism.kundakci@hotmail.com
// Date: 19/03/2017 13:15:00
// Description: OrangeDNS is a powerfull dns firewall solution written by C#
#endregion
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrangeDNS.Data.Migrations
{
    class Configuration : CreateDatabaseIfNotExists<OrangeDNSDataContext>
    {
        protected override void Seed(OrangeDNSDataContext context)
        {
            context.GeneralSettings.Add(new GeneralSettings() { Name = "GlobalBlock", Value = "0" });
            context.GeneralSettings.Add(new GeneralSettings() { Name = "GlobalBlockIp", Value = "127.0.0.1" });
            context.GeneralSettings.Add(new GeneralSettings() { Name = "MainDns", Value = "8.8.8.8" });
            context.GeneralSettings.Add(new GeneralSettings() { Name = "UpdateTime", Value = "30000" });
            //the test user, username: ismkdc , password:123
            context.Users.Add(new User { Username = "ismkdc", Password = "40bd001563085fc35165329ea1ff5c5ecbdbbeef", Name = "İsmail Kundakcı" });
            context.Categories.Add(new Category() { Name = "Haber" });
            context.Categories.Add(new Category() { Name = "Sosyal Medya" });
            context.Categories.Add(new Category() { Name = "Video Siteleri" });
            context.Categories.Add(new Category() { Name = "Diger" });
            base.Seed(context);
        }
    }
}
