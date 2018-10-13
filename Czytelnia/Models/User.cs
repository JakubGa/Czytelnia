using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Czytelnia.Models
{
    public class User
    {
        public int ID { get; set; }
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public DateTime Zapisany_od { get; set; }
        public ICollection<Book> Books { get; set; }
    }
}
