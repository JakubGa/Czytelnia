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
            new User{Imie="Jan",Nazwisko="Kowalski",Zapisany_od=DateTime.Parse("2017-09-16"),Books = new List<Book>() },
            new User{Imie="Henryk",Nazwisko="Nowak",Zapisany_od=DateTime.Parse("2015-09-15"),Books = new List<Book>() },
            new User{Imie="Janusz",Nazwisko="Cebularz",Zapisany_od=DateTime.Parse("2018-10-14"),Books = new List<Book>() }
            };
            foreach (User u in users)
            {
                context.Users.Add(u);
            }
            context.SaveChanges();

            var authors = new Author[]
            {
            new Author{Imie="Henryk",Nazwisko="Sienkiewicz" },
            new Author{Imie="Terry",Nazwisko="Pratchet" },
            new Author{Imie="Remigiusz",Nazwisko="Mróz" },
            new Author{Imie="Adam",Nazwisko="Mickiewicz" },
            new Author{Imie="J.R.R.",Nazwisko="Tolkien" },
            new Author{Imie="Andrzej",Nazwisko="Pilipiuk" }
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
            new Book{Tytul="Równoumagicznienie",Autor=context.Authors.Find(2),Gatunek=Gatunek.Fantastyka},
            new Book{Tytul="Hashtag",Autor=context.Authors.Find(3),Gatunek=Gatunek.Kryminal},
            new Book{Tytul="Pan Tadeusz",Autor=context.Authors.Find(4),Gatunek=Gatunek.Literatura_Piekna},
            new Book{Tytul="Władca Pierścieni. Drużyna Pierścienia",Autor=context.Authors.Find(5),Gatunek=Gatunek.Fantastyka},
            new Book{Tytul="Władca Pierścieni. Dwie Wieże",Autor=context.Authors.Find(5),Gatunek=Gatunek.Fantastyka},
            new Book{Tytul="Władca Pierścieni. Powrót Króla",Autor=context.Authors.Find(5),Gatunek=Gatunek.Fantastyka},
            new Book{Tytul="Wampir z M-3",Autor=context.Authors.Find(6),Gatunek=Gatunek.Fantastyka},
            new Book{Tytul="Konan Destylator",Autor=context.Authors.Find(6),Gatunek=Gatunek.Fantastyka}
            };
            foreach (Book b in books)
            {
                context.Books.Add(b);
            }
            context.SaveChanges();
        }
    }
}
