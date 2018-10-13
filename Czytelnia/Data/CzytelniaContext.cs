using Czytelnia.Models;
using Microsoft.EntityFrameworkCore;

namespace Czytelnia.Data
{
    public class CzytelniaContext : DbContext
    {
        public CzytelniaContext(DbContextOptions<CzytelniaContext> options) : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Author> Authors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>().ToTable("Book");
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Author>().ToTable("Author");
        }

    }
}