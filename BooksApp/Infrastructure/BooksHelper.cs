using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using BooksApp.Infrastructure.Providers;
using Dapper;

namespace BooksApp.Infrastructure
{
    public static class BooksHelper
    {
        private static readonly string ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public static bool ImportBooksFromXmlFile(string bookXmlFilePath)
        {
            try
            {
                IBooksProvider provider = new XmlBooksProvider(bookXmlFilePath);
                List<Book> books = provider.GetBooks().ToList();

                if (books.Any())
                {
                    string providerName = provider.GetType().Name;
                    return InsertBooksIntoDatabase(books, providerName);
                }

                return true;
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

            // Return true to indicate that no problems occured during actual importing of Books into the database.
            return true;
        }

        public static IEnumerable<Book> GetBooksFromDatabase(string searchString = null)
        {
            using (var sqlConnection = new SqlConnection(ConnectionString))
            {
                sqlConnection.Open();

                string query =
                    searchString == null
                        ? @"SELECT * FROM Books"
                        : string.Format(@"SELECT * FROM Books WHERE ISBN LIKE {0} OR Author LIKE {0} OR Title LIKE {0}", "'%' + @SearchString + '%'");

                return sqlConnection.Query<Book>(query, new { SearchString = searchString });
            }
        }

        public static IEnumerable<Provider> GetProvidersForBook(Guid bookId)
        {
            using (var sqlConnection = new SqlConnection(ConnectionString))
            {
                sqlConnection.Open();

                return sqlConnection.Query<Provider>(
                    @"
SELECT
    p.*
FROM
    Providers AS p
    INNER JOIN BookProviders as bp
        ON bp.ProviderId = p.Id
WHERE
    bp.BookId = @BookId",
                    new { BookId = bookId });
            }
        }

        #region Private Methods

        private static bool InsertBooksIntoDatabase(IEnumerable<Book> books, string providerName)
        {
            try
            {
                using (var sqlConnection = new SqlConnection(ConnectionString))
                {
                    sqlConnection.Open();

                    // Insert a Provider into database if it doesn't exist.

                    var provider = new Provider { Name = providerName };

                    object value = sqlConnection.ExecuteScalar(@"SELECT Id FROM Providers WHERE Name = @Name", provider);
                    if (value != null)
                    {
                        provider.Id = (Guid)value;
                    }
                    else
                    {
                        provider.Id = Guid.NewGuid();
                        sqlConnection.Execute(@"INSERT INTO Providers (Id, Name) VALUES (@Id, @Name)", provider);
                    }

                    foreach (Book book in books)
                    {
                        // Insert a Book into database if it doesn't exist.

                        value = sqlConnection.ExecuteScalar(
                            @"
SELECT
    Id
FROM
    Books
WHERE
    ISBN = @ISBN
    AND Author = @Author
    AND Title = @Title",
                            book);
                        if (value != null)
                        {
                            book.Id = (Guid)value;
                        }
                        else
                        {
                            book.Id = Guid.NewGuid();
                            sqlConnection.Execute(@"INSERT INTO Books (Id, ISBN, Author, Title) VALUES (@Id, @ISBN, @Author, @Title)", book);
                        }

                        var bookProvider = new BookProvider
                                           {
                                               BookId = book.Id,
                                               ProviderId = provider.Id,
                                           };

                        // Insert a relationship between Book and Provider if it doesn't exist.
                        int count = (int)sqlConnection.ExecuteScalar(
                            @"SELECT COUNT(*) FROM BookProviders WHERE BookId = @BookId AND ProviderId = @ProviderId",
                            bookProvider);
                        if (count < 1)
                        {
                            sqlConnection.Execute(@"INSERT INTO BookProviders (BookId, ProviderId) VALUES (@BookId, @ProviderId)", bookProvider);
                        }
                    }

                    return true;
                }
            }
            catch (Exception exception)
            {
                // ToDo: log
                // If anything unexpected happened, return false to signal that not all of the Books were successfully inserted.
                return false;
            }
        }

        #endregion
    }
}