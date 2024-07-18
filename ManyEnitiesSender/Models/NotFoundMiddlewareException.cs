namespace ManyEntitiesSender.Models
{
    public class NotFoundMiddlewareException : Exception
    {
        public NotFoundMiddlewareException(string message = "UseMyCaching must be after UseMyCachingValidation") : base(message)
        {
            
        }
    }
}
