using System;
using System.ComponentModel.DataAnnotations;

namespace LogReaderApp.Models
{
    public class Log : BaseModel
    {       
        public DateTime LogDate { get; set; }
        [StringLength(5)]
        public string Method { get; set; }
        [StringLength(200)]
        public string URL { get; set; }
        [StringLength(10)]
        public string Protocol { get; set; }
        [StringLength(3)]
        public string Result { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
    }
}
