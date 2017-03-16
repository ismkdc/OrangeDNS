using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrangeDNS.Data
{
   public class Client
    {
        [Key]
        public string Ip { get; set; }
        public string Name { get; set; }
        public bool IsBlocked { get; set; }
        public virtual ICollection<Log> Logs { get; set; }
        public Client()
        {
            Logs = new List<Data.Log>();
        }
    }
}
