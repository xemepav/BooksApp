using System.Collections.Generic;

namespace BooksApp.Models
{
    public class IndexModel
    {
        public IEnumerable<BookModel> Books { get; set; }
        public string SearchString { get; set; }
    }
}