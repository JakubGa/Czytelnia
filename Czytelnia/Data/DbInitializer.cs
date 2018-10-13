using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Czytelnia.Models;

namespace Czytelnia.Data
{
    public class DbInitializer
    {
        public static void Initialize(CzytelniaContext context)
        {
            context.Database.EnsureCreated();

            // Look for any students.
            if (context.Users.Any())
            {
                return;   // DB has been seeded
            }

            var users = new User[]
            {
            new User{Imie="Jan",Nazwisko="Kowalski",Zapisany_od=DateTime.Parse("2017-09-16") },
            new User{Imie="Henryk",Nazwisko="Nowak",Zapisany_od=DateTime.Parse("2015-09-15") }
            };
            foreach (User u in users)
            {
                context.Users.Add(u);
            }
            context.SaveChanges();

            var authors = new Author[]
            {
            new Author{Imie="Henryk",Nazwisko="Sienkiewicz" },
            new Author{Imie="Terry",Nazwisko="Pratchet" }
            };
            foreach (Author a in authors)
            {
                context.Authors.Add(a);
            }
            context.SaveChanges();

            var books = new Book[]
            {
            new Book{Tytul="Ogniem i mieczem",Autor=context.Authors.Find(1),Gatunek=Gatunek.Literatura_Piekna},
            new Book{Tytul="Quo vadis",Autor=context.Authors.Find(1),Gatunek=Gatunek.Literatura_Piekna},
            new Book{Tytul="W pustyni i w puszczy",Autor=context.Authors.Find(1),Gatunek=Gatunek.Literatura_Piekna},
            new Book{Tytul="Mort",Autor=context.Authors.Find(2),Gatunek=Gatunek.Fantastyka},
            new Book{Tytul="Równoumagicznienie",Autor=context.Authors.Find(2),Gatunek=Gatunek.Fantastyka}
            };
            foreach (Book b in books)
            {
                context.Books.Add(b);
            }
            context.SaveChanges();
        }
    }
}
