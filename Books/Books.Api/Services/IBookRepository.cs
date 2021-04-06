using Books.Api.Entities;
using Books.Api.ExternalModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Books.Api.Services
{
    public interface IBookRepository
    {
        IEnumerable<Entities.Book> GetBooks();
        //Entities.Book GetBook(Guid id);
        Task<IEnumerable<Book>> GetBooksAsync();
        Task<Book> GetBookAsync(Guid id);
        Task<IEnumerable<Book>> GetBooksAsync(IEnumerable<Guid> ids);
        void AddBook(Book bookToAdd);
        Task<BookCover> GetBookCoverAsync(string coverId);
        Task<IEnumerable<BookCover>> GetBookCoversAsync(Guid bookId);
        Task<bool> SaveChangesAsync();
    }
}
