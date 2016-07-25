using System.Collections.Generic;
using BooksApp.Interfaces.Entities;

namespace BooksApp.Interfaces
{
    public interface IBooksProvider
    {
        IEnumerable<IBook> GetBooks();
        string GetName();
    }
}