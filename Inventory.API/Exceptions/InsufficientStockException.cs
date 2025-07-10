namespace Inventory.API.Exceptions
{
    public class InsufficientStockException : Exception
    {
        public int ProductId { get; }
        public string ProductName { get; }
        public decimal AvailableQuantity { get; }
        public decimal RequestedQuantity { get; }

        public InsufficientStockException(int productId, string productName,
            decimal availableQuantity, decimal requestedQuantity)
            : base($"Insufficient stock for product '{productName}'. Available: {availableQuantity}, Requested: {requestedQuantity}")
        {
            ProductId = productId;
            ProductName = productName;
            AvailableQuantity = availableQuantity;
            RequestedQuantity = requestedQuantity;
        }
    }

   
}
