using Inventory.API.Models;

namespace Inventory.API.Interfaces
{
    public interface ICustomerService
    {
        // Basic CRUD operations
        Task<Customer> GetCustomerByIdAsync(int id);
        Task<IEnumerable<Customer>> GetAllCustomersAsync();
        Task<Customer> AddCustomerAsync(Customer customer);
        Task<Customer> UpdateCustomerAsync(int id, Customer customer);
        Task<bool> DeleteCustomerAsync(int id);

        // Customer-specific operations
        Task<IEnumerable<Customer>> SearchCustomersAsync(string searchTerm);
        Task<int> GetCustomerLoyaltyPointsAsync(int customerId);
        Task<bool> UpdateLoyaltyPointsAsync(int customerId, int pointsChange);

        // Sales-related operations
        Task<IEnumerable<Sale>> GetCustomerSalesAsync(int customerId);
        Task<decimal> GetCustomerTotalSpendingAsync(int customerId);

        // Pagination and filtering
        Task<(IEnumerable<Customer> customers, int totalCount)> GetCustomersPagedAsync(
            int pageNumber,
            int pageSize,
            string searchTerm = null);
    }
}
