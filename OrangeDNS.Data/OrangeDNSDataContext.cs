using OrangeDNS.Data.Migrations;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrangeDNS.Data
{
    public class OrangeDNSDataContext : DbContext
    {
        public OrangeDNSDataContext() : base("OrangeDNS")
        {
            Database.SetInitializer(new Configuration());
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Domain> Domains { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<GeneralSettings> GeneralSettings { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Client> Clients { get; set; }
    }
}
