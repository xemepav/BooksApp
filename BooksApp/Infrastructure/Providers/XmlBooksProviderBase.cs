using System;
using System.Collections.Generic;
using System.IO;

namespace BooksApp.Infrastructure.Providers
{
    public abstract class XmlBooksProviderBase : IBooksProvider
    {
        protected string BooksXml;

        protected XmlBooksProviderBase(string booksXmlFilePath)
        {
            if (string.IsNullOrWhiteSpace(booksXmlFilePath))
            {
                throw new ArgumentNullException("booksXmlFilePath");
            }

            if (!File.Exists(booksXmlFilePath))
            {
                throw new BooksXmlNotFoundException();
            }

            BooksXml = File.ReadAllText(booksXmlFilePath);
        }

        public abstract IEnumerable<Book> GetBooks();
    }
}