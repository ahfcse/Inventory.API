using Inventory.API.Data;
using Inventory.API.Interfaces;
using Inventory.API.Models;


namespace Inventory.API.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private bool _disposed;

        public UnitOfWork(AppDbContext context)
        {

            _context = context;
        }

        public IRepository<Product> Products => new Repository<Product>(_context);
        public IRepository<Customer> Customers => new Repository<Customer>(_context);
        public IRepository<Sale> Sales => new Repository<Sale>(_context);
        public IRepository<SaleDetail> SaleDetails => new Repository<SaleDetail>(_context);

        public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _context.Dispose();
            }
            _disposed = true;
        }
    }
}
