namespace BooksApp.DAL
{
    public abstract class SqlRepositoryBase
    {
        protected readonly string ConnectionString;

        protected SqlRepositoryBase(string connectionString)
        {
            ConnectionString = connectionString;
        }
    }
}