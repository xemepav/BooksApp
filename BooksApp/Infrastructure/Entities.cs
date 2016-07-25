using System;

namespace BooksApp.Infrastructure
{
    public class Book
    {
        public Guid Id { get; set; }
        public string Isbn { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
    }

    public class Provider
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class BookProvider
    {
        public Guid BookId { get; set; }
        public Guid ProviderId { get; set; }
    }
}