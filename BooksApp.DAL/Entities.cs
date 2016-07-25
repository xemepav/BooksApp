using System;
using BooksApp.Interfaces.Entities;

namespace BooksApp.DAL
{
    public class RepositoryBook : IRepositoryBook
    {
        public Guid Id { get; set; }
        public string Isbn { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
    }

    public class RepositoryProvider : IRepositoryProvider
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class RepositoryBookProvider : IRepositoryBookProvider
    {
        public Guid BookId { get; set; }
        public Guid ProviderId { get; set; }

        public RepositoryBookProvider(Guid bookId, Guid providerId)
        {
            BookId = bookId;
            ProviderId = providerId;
        }
    }
}