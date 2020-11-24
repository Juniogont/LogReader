using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LogReaderApp.Models
{
    public class BaseModel
    {
        public int Id { get; set; }
        [StringLength(20)]
        [Required]
        public string IP { get; set; }
    }
}
