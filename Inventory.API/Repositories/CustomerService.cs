using Inventory.API.Interfaces;
using Inventory.API.Models;

namespace Inventory.API.Repositories
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CustomerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Customer> AddCustomerAsync(Customer customer)
        {
            // Validate email uniqueness
            if (!string.IsNullOrEmpty(customer.Email))
            {
                var existingCustomer = (await _unitOfWork.Customers.FindAsync(
                    c => c.Email == customer.Email)).FirstOrDefault();

                if (existingCustomer != null)
                {
                    throw new Exception("Email address already exists");
                }
            }

            await _unitOfWork.Customers.AddAsync(customer);
            await _unitOfWork.CompleteAsync();
            return customer;
        }

        public async Task<bool> DeleteCustomerAsync(int id)
        {
            var customer = await _unitOfWork.Customers.GetByIdAsync(id);
            if (customer == null) return false;

            // Soft delete implementation
            customer.IsDeleted = true;
            _unitOfWork.Customers.Update(customer);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            return await _unitOfWork.Customers.GetAllAsync();
        }

        public async Task<Customer> GetCustomerByIdAsync(int id)
        {
            return await _unitOfWork.Customers.GetByIdAsync(id);
        }

        public async Task<int> GetCustomerLoyaltyPointsAsync(int customerId)
        {
            var customer = await _unitOfWork.Customers.GetByIdAsync(customerId);
            return customer?.LoyaltyPoints ?? 0;
        }

        public async Task<IEnumerable<Sale>> GetCustomerSalesAsync(int customerId)
        {
            return await _unitOfWork.Sales.FindAsync(s => s.CustomerId == customerId);
        }

        public async Task<decimal> GetCustomerTotalSpendingAsync(int customerId)
        {
            var sales = await _unitOfWork.Sales.FindAsync(s => s.CustomerId == customerId);
            return sales.Sum(s => s.TotalAmount);
        }

        public async Task<(IEnumerable<Customer> customers, int totalCount)> GetCustomersPagedAsync(
            int pageNumber, int pageSize, string searchTerm = null)
        {
            var query = await _unitOfWork.Customers.GetAllAsync();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(c =>
                    c.FullName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    c.Email.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    c.Phone.Contains(searchTerm));
            }

            var totalCount = await _unitOfWork.Customers.CountAsync();
            var customers = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return (customers, totalCount);
        }

        public async Task<IEnumerable<Customer>> SearchCustomersAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetAllCustomersAsync();

            return await _unitOfWork.Customers.FindAsync(c =>
                c.FullName.Contains(searchTerm) ||
                c.Email.Contains(searchTerm) ||
                c.Phone.Contains(searchTerm));
        }

        public async Task<bool> UpdateLoyaltyPointsAsync(int customerId, int pointsChange)
        {
            var customer = await _unitOfWork.Customers.GetByIdAsync(customerId);
            if (customer == null) return false;

            customer.LoyaltyPoints += pointsChange;
            _unitOfWork.Customers.Update(customer);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<Customer> UpdateCustomerAsync(int id, Customer customer)
        {
            var existingCustomer = await _unitOfWork.Customers.GetByIdAsync(id);
            if (existingCustomer == null) return null;

            // Validate email uniqueness if changed
            if (!string.IsNullOrEmpty(customer.Email) &&
                existingCustomer.Email != customer.Email)
            {
                var customerWithEmail = (await _unitOfWork.Customers.FindAsync(
                    c => c.Email == customer.Email)).FirstOrDefault();

                if (customerWithEmail != null)
                {
                    throw new Exception("Email address already exists");
                }
            }

            // Update properties
            existingCustomer.FullName = customer.FullName;
            existingCustomer.Phone = customer.Phone;
            existingCustomer.Email = customer.Email;
            existingCustomer.LoyaltyPoints = customer.LoyaltyPoints;

            _unitOfWork.Customers.Update(existingCustomer);
            await _unitOfWork.CompleteAsync();
            return existingCustomer;
        }
    }
}
