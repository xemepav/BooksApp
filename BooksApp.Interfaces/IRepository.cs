using System;
using System.Collections.Generic;
using BooksApp.Interfaces.Entities;

namespace BooksApp.Interfaces
{
    public interface IRepository
    {
        IEnumerable<IRepositoryBook> GetBooks(string searchString);
        void InsertBooks(IEnumerable<IBook> books, string providerName);

        Guid InsertBook(IBook book);
        Guid InsertProvider(IProvider provider);

        IEnumerable<IRepositoryProvider> GetProvidersForBook(Guid bookId);
        void AddProviderForBook(Guid bookId, Guid providerId);
    }
}