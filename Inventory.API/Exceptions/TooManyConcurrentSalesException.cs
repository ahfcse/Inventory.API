namespace Inventory.API.Exceptions
{
    public class TooManyConcurrentSalesException : Exception
    {
        public TooManyConcurrentSalesException()
            : base("Too many concurrent sales requests. Please try again later.")
        {
        }

        public int StatusCode => 429;
    }
}
