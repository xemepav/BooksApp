namespace BooksApp.Interfaces.Entities
{
    public interface IBook
    {
        string Isbn { get; set; }
        string Author { get; set; }
        string Title { get; set; }
    }

    public interface IProvider
    {
        string Name { get; set; }
    }
}