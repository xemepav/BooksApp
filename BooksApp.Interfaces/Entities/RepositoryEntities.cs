using System;

namespace BooksApp.Interfaces.Entities
{
    public interface IRepositoryBook : IBook
    {
        Guid Id { get; set; }
    }

    public interface IRepositoryProvider : IProvider
    {
        Guid Id { get; set; }
    }

    public interface IRepositoryBookProvider
    {
        Guid BookId { get; set; }
        Guid ProviderId { get; set; }
    }
}