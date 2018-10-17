using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Czytelnia.Models
{
    public class Author
    {
        public int AuthorID { get; set; }
        [Display(Name="Imię")]
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
    }
}
