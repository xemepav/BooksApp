using System;
using System.Collections.Generic;
using System.Linq;
using BooksApp.BLL.Providers.XmlProviders;
using BooksApp.Interfaces;
using BooksApp.Interfaces.Entities;

namespace BooksApp.BLL.Helpers
{
    public class BooksHelper : IBooksHelper
    {
        private readonly IRepository _repository;

        public BooksHelper(IRepository repository)
        {
            _repository = repository;
        }

        public bool ImportBooksFromXmlFile(string bookXmlFilePath)
        {
            try
            {
                IBooksProvider provider = new XmlBooksProvider(bookXmlFilePath, "Dummy");
                List<IBook> books = provider.GetBooks().ToList();

                if (books.Any())
                {
                    try
                    {
                        _repository.InsertBooks(books, provider.GetName());
                    }
                    catch (Exception exception)
                    {
                        // ToDo: log
                        // If anything unexpected happened, return false to signal that not all of the Books were successfully imported.
                        return false;
                    }
                }
            }
            catch (BooksXmlInvalidException exception)
            {
                // ToDo: log
            }
            catch (BooksXmlNotFoundException exception)
            {
                // ToDo: log
            }
            catch (Exception exception)
            {
                // ToDo: log
            }

            // Return true to indicate that no problems occured during importing of Books into the repository (disregarding any problems with the XML file itself).
            return true;
        }
    }
}