using Inventory.API.Interfaces;
using Inventory.API.Models;

namespace Inventory.API.Repositories
{
    // ProductService.cs
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Product> AddProduct(Product product)
        {
            // Check for unique barcode using repository
            var existingProduct = (await _unitOfWork.Products.FindAsync(p => p.Barcode == product.Barcode)).FirstOrDefault();
            if (existingProduct != null)
                throw new Exception("Barcode must be unique");

            await _unitOfWork.Products.AddAsync(product);
            await _unitOfWork.CompleteAsync();
            return product;
        }

        public async Task<Product> GetProductById(int id)
        {
            return await _unitOfWork.Products.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Product>> GetProducts(ProductFilter filter)
        {
            var query = await _unitOfWork.Products.GetAllAsync();

            // Apply filters
            if (!string.IsNullOrEmpty(filter.Name))
                query = query.Where(p => p.Name.Contains(filter.Name));

            if (!string.IsNullOrEmpty(filter.Category))
                query = query.Where(p => p.Category == filter.Category);

            if (filter.Status.HasValue)
                query = query.Where(p => p.Status == filter.Status);

            // Apply pagination
            if (filter.PageNumber > 0 && filter.PageSize > 0)
                query = query.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize);

            return query.ToList();
        }

        Task<Product> IProductService.AddProductAsync(Product product)
        {
            throw new NotImplementedException();
        }

        Task<bool> IProductService.DeleteProductAsync(int id)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<Product>> IProductService.GetActiveProductsAsync()
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<Product>> IProductService.GetAllProductsAsync()
        {
            throw new NotImplementedException();
        }

        Task<Product> IProductService.GetProductByBarcodeAsync(string barcode)
        {
            throw new NotImplementedException();
        }

        Task<Product> IProductService.GetProductByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<Product>> IProductService.GetProductsByCategoryAsync(string category)
        {
            throw new NotImplementedException();
        }

        Task<(IEnumerable<Product> products, int totalCount)> IProductService.GetProductsPagedAsync(int pageNumber, int pageSize, string searchTerm, string category, bool? status)
        {
            throw new NotImplementedException();
        }

        Task<decimal> IProductService.GetProductStockQuantityAsync(int productId)
        {
            throw new NotImplementedException();
        }

        Task<Product> IProductService.UpdateProductAsync(int id, Product product)
        {
            throw new NotImplementedException();
        }

        Task<bool> IProductService.UpdateStockQuantityAsync(int productId, decimal quantityChange)
        {
            throw new NotImplementedException();
        }

        // Other CRUD operations...
    }
}
