using Microsoft.EntityFrameworkCore;

namespace ct_datenbanken.Data
{
    public class LibraryDbContext : DbContext
    {
        protected LibraryDbContext()
        { }

        public LibraryDbContext(DbContextOptions options) : base(options)
        {
            Database.Migrate();
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