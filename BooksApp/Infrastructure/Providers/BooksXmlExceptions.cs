using System;

namespace BooksApp.Infrastructure.Providers
{
    public class BooksXmlNotFoundException : Exception
    {
        public BooksXmlNotFoundException()
        {
            
        }

        public BooksXmlNotFoundException(string message)
            : base(message)
        {

        }
        public BooksXmlNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }

    public class BooksXmlInvalidException : Exception
    {
        public BooksXmlInvalidException()
        {
            
        }

        public BooksXmlInvalidException(string message)
            : base(message)
        {

        }
        public BooksXmlInvalidException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}