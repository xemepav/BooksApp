using System.Collections.Generic;
using BooksApp.Interfaces;
using BooksApp.Interfaces.Entities;

namespace BooksApp.BLL.Providers
{
    public abstract class BooksProviderBase : IBooksProvider
    {
        protected readonly string ProviderName;

        protected BooksProviderBase(string providerName = null)
        {
            ProviderName = providerName;
        }

        public virtual string GetName()
        {
            return ProviderName;
        }

        public abstract IEnumerable<IBook> GetBooks();
    }
}