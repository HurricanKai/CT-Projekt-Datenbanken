using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ct_datenbanken.Data
{
    public class LibraryDbContext : DbContext
    {
        protected LibraryDbContext()
        {
            Database.Migrate();
        }

        public LibraryDbContext(DbContextOptions options) : base(options)
        {
            Database.Migrate();
        }

        public void AddBooks()
        {
            var author = Authors.First();
            Books.AddRange(Enumerable.Range(0, 1000).Select(x => new Book
            {
                Title = $"Book {x}",
                PageCount = x,
                Author = author,
                IsAvailable = true,
                Description = "Auto generated Book"
            }));

            SaveChanges();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder
                .Entity<Author>(builder => { builder.HasKey(x => x.Id); })
                .Entity<Book>(builder =>
                {
                    builder.HasKey(x => x.Id);
                    builder.HasOne(x => x.Author).WithMany(x => x.Books);
                });
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
    }
}