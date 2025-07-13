namespace Inventory.API.Exceptions
{
    public class ProductNotFoundException : Exception
    {
        /// <summary>
        /// ID of the product that was not found
        /// </summary>
        public int ProductId { get; }

        /// <summary>
        /// Creates a new instance of ProductNotFoundException
        /// </summary>
        /// <param name="productId">ID of the product that was not found</param>
        public ProductNotFoundException(int productId)
            : base($"Product with ID {productId} not found or has been deleted")
        {
            ProductId = productId;
        }

        /// <summary>
        /// Creates a new instance of ProductNotFoundException with custom message
        /// </summary>
        /// <param name="productId">ID of the product that was not found</param>
        /// <param name="message">Custom error message</param>
        public ProductNotFoundException(int productId, string message)
            : base(message)
        {
            ProductId = productId;
        }

        /// <summary>
        /// Creates a new instance of ProductNotFoundException with inner exception
        /// </summary>
        /// <param name="productId">ID of the product that was not found</param>
        /// <param name="message">Error message</param>
        /// <param name="innerException">Inner exception</param>
        public ProductNotFoundException(int productId, string message, Exception innerException)
            : base(message, innerException)
        {
            ProductId = productId;
        }
    }
}
