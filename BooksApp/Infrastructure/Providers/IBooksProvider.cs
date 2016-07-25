using System.Collections.Generic;

namespace BooksApp.Infrastructure.Providers
{
    public interface IBooksProvider
    {
        IEnumerable<Book> GetBooks();
    }
}