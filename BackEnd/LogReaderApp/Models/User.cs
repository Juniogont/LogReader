using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LogReaderApp.Models
{
    public class User : BaseModel
    {
        [StringLength(50)]
        public string Nome { get; set; }
    }
}
