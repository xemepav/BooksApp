using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BooksApp.Infrastructure;
using BooksApp.Models;

namespace BooksApp.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            var model = new IndexModel
                        {
                            Books = GetBooks(),
                        };
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(IndexModel model)
        {
            model.Books = GetBooks(model.SearchString);
            return View(model);
        }

        private IEnumerable<BookModel> GetBooks(string searchString = null)
        {
            return BooksHelper.GetBooksFromDatabase(searchString).Select(
                book => new BookModel
                        {
                            Isbn = book.Isbn,
                            Author = book.Author,
                            Title = book.Title,
                            Providers = string.Join(", ", BooksHelper.GetProvidersForBook(book.Id).Select(prov => prov.Name)),
                        });
        }
    }
}