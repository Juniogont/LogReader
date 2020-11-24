using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LogReader.UI.Models
{
    public class Log
    {
        public int Id { get; set; }
        [StringLength(20)]
        [Required]
        public string IP { get; set; }
        [Display(Name = "Data do Log")]
        public DateTime LogDate { get; set; }
        [StringLength(5)]
        [Display(Name = "Método")]
        public string Method { get; set; }
        [StringLength(200)]
        public string URL { get; set; }
        [StringLength(10)]
        [Display(Name = "Protocolo")]
        public string Protocol { get; set; }
        [StringLength(3)]
        public string Result { get; set; }
        public User User { get; set; }
        [Display(Name = "Usuário")]
        public int UserId { get; set; }
    }
}
