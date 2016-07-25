using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using BooksApp.Interfaces;
using BooksApp.Interfaces.Entities;
using Dapper;

namespace BooksApp.DAL
{
    public class SqlRepository : SqlRepositoryBase, IRepository
    {
        public SqlRepository(string connectionString)
            : base(connectionString)
        {

        }

        public IEnumerable<IRepositoryBook> GetBooks(string searchString = null)
        {
            using (var sqlConnection = new SqlConnection(ConnectionString))
            {
                sqlConnection.Open();

                string selectQuery = SelectBooksQuery;
                string whereClause = searchString == null
                    ? string.Empty
                    : string.Format(
                        @"
WHERE
    b.ISBN LIKE {0}
    OR Author LIKE {0}
    OR Title LIKE {0}",
                        "'%' + @SearchString + '%'");

                string query = selectQuery + whereClause;
                return sqlConnection.Query<RepositoryBook>(query, new { SearchString = searchString });
            }
        }

        public void InsertBooks(IEnumerable<IBook> books, string providerName)
        {
            // Insert a Provider into database if it doesn't exist.
            Guid providerId = InsertProvider(new RepositoryProvider { Name = providerName });

            foreach (IBook book in books)
            {
                // Insert a Book into database if it doesn't exist.
                Guid bookId = InsertBook(book);

                // Insert a relationship between Book and Provider if it doesn't exist.
                AddProviderForBook(bookId, providerId);
            }
        }

        public Guid InsertBook(IBook book)
        {
            using (var sqlConnection = new SqlConnection(ConnectionString))
            {
                sqlConnection.Open();

                var repositoryBook = new RepositoryBook
                                     {
                                         Isbn = book.Isbn,
                                         Author = book.Author,
                                         Title = book.Title,
                                     };

                object value = sqlConnection.ExecuteScalar(SelectBooksByFieldsQuery, book);
                if (value != null)
                {
                    repositoryBook.Id = (Guid)value;
                }
                else
                {
                    repositoryBook.Id = Guid.NewGuid();
                    sqlConnection.Execute(InsertIntoBooksQuery, repositoryBook);
                }

                return repositoryBook.Id;
            }
        }

        public Guid InsertProvider(IProvider provider)
        {
            using (var sqlConnection = new SqlConnection(ConnectionString))
            {
                sqlConnection.Open();

                var repositoryProvider = new RepositoryProvider
                                         {
                                             Name = provider.Name,
                                         };

                object value = sqlConnection.ExecuteScalar(SelectProvidersByNameQuery, provider);
                if (value != null)
                {
                    repositoryProvider.Id = (Guid)value;
                }
                else
                {
                    repositoryProvider.Id = Guid.NewGuid();
                    sqlConnection.Execute(InsertIntoProvidersQuery, repositoryProvider);
                }

                return repositoryProvider.Id;
            }
        }

        public IEnumerable<IRepositoryProvider> GetProvidersForBook(Guid bookId)
        {
            using (var sqlConnection = new SqlConnection(ConnectionString))
            {
                sqlConnection.Open();

                return sqlConnection.Query<RepositoryProvider>(SelectProvidersByBookIdQuery, new { BookId = bookId });
            }
        }

        public void AddProviderForBook(Guid bookId, Guid providerId)
        {
            using (var sqlConnection = new SqlConnection(ConnectionString))
            {
                var bookProvider = new RepositoryBookProvider(bookId, providerId);

                int count = (int)sqlConnection.ExecuteScalar(SelectCountBookProvidersQuery, bookProvider);
                if (count < 1)
                {
                    sqlConnection.Execute(InsertIntoBookProvidersQuery, bookProvider);
                }
            }
        }

        #region SQL Queries

        private const string SelectBooksQuery = @"
SELECT
     b.Id
    ,b.ISBN
    ,b.Author
    ,b.Title
FROM
    Books AS b";

        private const string SelectBooksByFieldsQuery = @"
SELECT
     b.Id
FROM
    Books AS b
WHERE
    ISBN = @Isbn
    AND Author = @Author
    AND Title = @Title";

        private const string InsertIntoBooksQuery = @"
INSERT INTO Books
(
     Id
    ,ISBN
    ,Author
    ,Title
)
VALUES
(
     @Id
    ,@ISBN
    ,@Author
    ,@Title
)";

        private const string SelectProvidersByNameQuery = @"
SELECT
     p.Id
FROM
    Providers AS p
WHERE
    Name = @Name";

        private const string InsertIntoProvidersQuery = @"
INSERT INTO Providers
(
     Id
    ,Name
)
VALUES
(
     @Id
    ,@Name
)";

        private const string SelectProvidersByBookIdQuery = @"
SELECT
     p.Id
    ,p.Name
FROM
    Providers AS p
    INNER JOIN BookProviders as bp
        ON bp.ProviderId = p.Id
WHERE
    bp.BookId = @BookId";

        private const string SelectCountBookProvidersQuery = @"
SELECT
    COUNT(*)
FROM
    BookProviders AS bp
WHERE
    bp.BookId = @BookId
    AND bp.ProviderId = @ProviderId";

        private const string InsertIntoBookProvidersQuery = @"
INSERT INTO BookProviders
(
     BookId
    ,ProviderId
)
VALUES
(
     @BookId
    ,@ProviderId
)";

        #endregion
    }
}