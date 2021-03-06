﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Czytelnia.Models
{
    public enum Gatunek
    {
        Literatura_Piekna, Kryminal, Romans, Fantastyka
    }
    public class Book
    {
        public int BookID { get; set; }
        [Display(Name="Tytuł")]
        public string Tytul { get; set; }
        public int AuthorID { get; set; }
        public Author Autor { get; set; }
        public Gatunek Gatunek { get; set; }
        public int? UserID { get; set; }
    }
}
