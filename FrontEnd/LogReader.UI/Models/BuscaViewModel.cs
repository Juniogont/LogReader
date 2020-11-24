using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LogReader.UI.Models
{
    public class BuscaViewModel
    {
       public IEnumerable<Log> Logs { get; set; }
        public string IP { get; set; }
        public User User { get; set; }
        [Display(Name = "Usuário")]
        public int UserId { get; set; }
    }
}
