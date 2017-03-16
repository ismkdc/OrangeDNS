
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrangeDNS.Data
{
    public class Log
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("ClientIp")]
        public virtual Client Client { get; set; }
        public string ClientIp { get; set; }
        public string Request { get; set; }
        public ResultType Result { get; set; }
        public DateTime Date { get; set; }
        public Log()
        {
            Date = DateTime.Now;
        }

        public enum ResultType
        {
            success,
            blocked,
            forward
        }
    }
}
