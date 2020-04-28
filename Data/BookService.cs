using System.Linq;
using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ct_datenbanken.Data
{
    public sealed class BookService
    {
        private readonly LibraryDbContext _dbContext;

        public BookService(LibraryDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public void Create(BookDTO book)
        {
            var authorName = book.AuthorName;

            var authorEntry = GetOrCreateAuthor(authorName);

            var b = new Book
            {
                Title = book.Title,
                PageCount = book.PageCount,
                Author = authorEntry.Entity,
                IsAvailable = book.IsAvailable,
                Description = book.Description
            };

            var bookEntry = _dbContext.Books.Add(b);
            authorEntry.Entity.Books.Add(bookEntry.Entity);

            _dbContext.SaveChanges();
        }

        private EntityEntry<Author> GetOrCreateAuthor(string authorName)
        {
            var author = _dbContext.Authors.Where((author => author.Name == authorName)).SingleOrDefault();

            EntityEntry<Author> authorEntry;
            if (author is null)
            {
                // Create author
                authorEntry = _dbContext.Authors.Add(new Author
                {
                    Name = authorName,
                });
            }
            else
            {
                authorEntry = _dbContext.Authors.Update(author);
            }

            return authorEntry;
        }

        public BookDTO Get(int id)
        {
            var book = _dbContext.Books.Find(id);

            if (book is null)
                return null;

            return new BookDTO
            {
                Id = book.Id,
                Title = book.Title,
                PageCount = book.PageCount,
                AuthorName = book.Author.Name,
                IsAvailable = book.IsAvailable,
                Description = book.Description
            };
        }

        public void Update(BookDTO dto)
        {
            var old = _dbContext.Books.Find(dto.Id);

            var oldEntry = _dbContext.Books.Update(old);

            oldEntry.Entity.Title = dto.Title;
            oldEntry.Entity.PageCount = dto.PageCount;
            oldEntry.Entity.IsAvailable = dto.IsAvailable;
            oldEntry.Entity.Description = dto.Description;
            
            if (old.Author.Name != dto.AuthorName)
            {
                old.Author.Books.Remove(old);

                var author = GetOrCreateAuthor(dto.AuthorName);
                author.Entity.Books.Add(oldEntry.Entity);
            }

            _dbContext.SaveChanges();
        }

        public void ToggleAvailability(int id)
        {
            var book = _dbContext.Books.Find(id);

            var bookEntry = _dbContext.Books.Update(book);
            bookEntry.Entity.IsAvailable = !bookEntry.Entity.IsAvailable;

            _dbContext.SaveChanges();
        }
    }
}