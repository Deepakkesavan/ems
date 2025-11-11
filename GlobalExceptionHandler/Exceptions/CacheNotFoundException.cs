namespace GlobalExceptionHandler.Exceptions
{
    public class CacheNotFoundException:Exception
    {
        public CacheNotFoundException(string key) : base($"Cache is not found for the key: {key}")
        {

        }
        
    }
}
