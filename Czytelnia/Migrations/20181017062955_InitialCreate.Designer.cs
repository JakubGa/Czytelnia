﻿// <auto-generated />
using System;
using Czytelnia.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Czytelnia.Migrations
{
    [DbContext(typeof(CzytelniaContext))]
    [Migration("20181017062955_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Czytelnia.Models.Author", b =>
                {
                    b.Property<int>("AuthorID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Imie");

                    b.Property<string>("Nazwisko");

                    b.HasKey("AuthorID");

                    b.ToTable("Author");
                });

            modelBuilder.Entity("Czytelnia.Models.Book", b =>
                {
                    b.Property<int>("BookID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AuthorID");

                    b.Property<int>("Gatunek");

                    b.Property<string>("Tytul");

                    b.Property<int?>("UserID");

                    b.HasKey("BookID");

                    b.HasIndex("AuthorID");

                    b.HasIndex("UserID");

                    b.ToTable("Book");
                });

            modelBuilder.Entity("Czytelnia.Models.User", b =>
                {
                    b.Property<int>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Imie");

                    b.Property<string>("Nazwisko");

                    b.Property<DateTime>("Zapisany_od");

                    b.HasKey("UserID");

                    b.ToTable("User");
                });

            modelBuilder.Entity("Czytelnia.Models.Book", b =>
                {
                    b.HasOne("Czytelnia.Models.Author", "Autor")
                        .WithMany()
                        .HasForeignKey("AuthorID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Czytelnia.Models.User")
                        .WithMany("Books")
                        .HasForeignKey("UserID");
                });
#pragma warning restore 612, 618
        }
    }
}
