using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LogReaderApp.Models
{
    public class ReturnLogModelwww
    {
        public DateTime LogDate { get; set; }
        [StringLength(5)]
        public string Method { get; set; }
        [StringLength(5)]
        public string URL { get; set; }
        [StringLength(10)]
        public string Protocol { get; set; }
        [StringLength(3)]
        public string Result { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
    }
}
