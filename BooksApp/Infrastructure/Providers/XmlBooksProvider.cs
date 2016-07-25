using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace BooksApp.Infrastructure.Providers
{
    public class XmlBooksProvider : XmlBooksProviderBase
    {
        public XmlBooksProvider(string booksXmlFilePath)
            : base(booksXmlFilePath)
        {

        }

        public override IEnumerable<Book> GetBooks()
        {
            try
            {
                XDocument doc = XDocument.Parse(BooksXml);

                return
                    from b in doc.Root.Descendants("Book")
                    select new Book
                           {
                               Isbn = b.Element("ISBN").Value,
                               Author = b.Element("Author").Value,
                               Title = b.Element("Title").Value,
                           };
            }
            catch (Exception exception)
            {
                throw new BooksXmlInvalidException("Books xml could not be parsed correctly.", exception);
            }
        }
    }
}