using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using BooksApp.DAL;
using BooksApp.Interfaces;
using BooksApp.Models;

namespace BooksApp.Controllers
{
    public class HomeController : Controller
    {
        private IRepository Repository { get; set; }

        public HomeController()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            Repository = new SqlRepository(connectionString);
        }

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
            return Repository.GetBooks(searchString).Select(
                book => new BookModel
                        {
                            Isbn = book.Isbn,
                            Author = book.Author,
                            Title = book.Title,
                            Providers = string.Join(", ", Repository.GetProvidersForBook(book.Id).Select(prov => prov.Name)),
                        });
        }
    }
}