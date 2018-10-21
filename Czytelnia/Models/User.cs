using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Czytelnia.Models
{
    public class User
    {
        public int UserID { get; set; }
        [Display(Name ="Imię")]
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        [Display(Name = "Zapisany od")]
        [DataType(DataType.Date)]
        public DateTime Zapisany_od { get; set; }
        [Display(Name ="Wypożyczone")]
        public ICollection<Book> Books { get; set; }
        
    }
}
