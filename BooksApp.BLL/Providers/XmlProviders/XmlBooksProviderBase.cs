using System;
using System.IO;

namespace BooksApp.BLL.Providers.XmlProviders
{
    public abstract class XmlBooksProviderBase : BooksProviderBase
    {
        protected string BooksXmlFilePath;

        protected XmlBooksProviderBase(string booksXmlFilePath, string providerName = null)
            : base(providerName)
        {
            if (string.IsNullOrWhiteSpace(booksXmlFilePath))
            {
                throw new ArgumentNullException("booksXmlFilePath");
            }

            if (!File.Exists(booksXmlFilePath))
            {
                throw new BooksXmlNotFoundException();
            }

            BooksXmlFilePath = booksXmlFilePath;
        }
    }
}