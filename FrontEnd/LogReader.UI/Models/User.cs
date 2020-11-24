using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LogReader.UI.Models
{
    public class User
    {
        public int Id { get; set; }
        [StringLength(20)]
        [Required]
        public string IP { get; set; }
        [StringLength(50)]
        public string Nome { get; set; }
    }
}
