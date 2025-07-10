using Inventory.API.Models;

namespace Inventory.API.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Product> Products { get; }
        IRepository<Customer> Customers { get; }
        IRepository<Sale> Sales { get; }
        IRepository<SaleDetail> SaleDetails { get; }
        Task<int> CompleteAsync();
    }
}
