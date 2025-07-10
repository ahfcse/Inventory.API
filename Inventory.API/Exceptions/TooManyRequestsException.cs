namespace Inventory.API.Exceptions
{
    public class TooManyRequestsException : Exception
    {
        public TooManyRequestsException()
            : base("Too many concurrent sale requests. Please try again later.")
        {
        }
    }
}
