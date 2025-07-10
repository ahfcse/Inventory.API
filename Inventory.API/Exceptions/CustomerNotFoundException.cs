namespace Inventory.API.Exceptions
{
    public class CustomerNotFoundException : Exception
    {
        public int CustomerId { get; }

        public CustomerNotFoundException(int customerId)
            : base($"Customer with ID {customerId} not found")
        {
            CustomerId = customerId;
        }
    }
}
