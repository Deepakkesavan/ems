namespace GlobalExceptionHandler.Exceptions
{
        public class SqlDependencyNotInitializedException : Exception
        {
            public SqlDependencyNotInitializedException(string message)
                : base(message) { }
        }

    }

