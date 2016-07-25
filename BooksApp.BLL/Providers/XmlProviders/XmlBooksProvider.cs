using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using BooksApp.Interfaces.Entities;

namespace BooksApp.BLL.Providers.XmlProviders
{
    public class XmlBooksProvider : XmlBooksProviderBase
    {
        public XmlBooksProvider(string booksXmlFilePath, string providerName = null)
            : base(booksXmlFilePath, providerName)
        {
            
        }

        public override string GetName()
        {
            return base.GetName() ?? GetType().Name;
        }

        public override IEnumerable<IBook> GetBooks()
        {
            try
            {
                var serializer = new XmlSerializer(typeof(XmlBookCollection));

                using (var fileStream = new FileStream(BooksXmlFilePath, FileMode.Open))
                {
                    XmlBookCollection xmlBookCollection = (XmlBookCollection)serializer.Deserialize(fileStream);
                    return xmlBookCollection.XmlBooks;
                }
            }
            catch (Exception exception)
            {
                throw new BooksXmlInvalidException("Books xml could not be parsed correctly.", exception);
            }
        }
    }
}