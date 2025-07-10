using Inventory.API.Models;

namespace Inventory.API.Interfaces
{
    public interface IProductService
    {
        // Basic CRUD operations
        Task<Product> GetProductByIdAsync(int id);
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product> AddProductAsync(Product product);
        Task<Product> UpdateProductAsync(int id, Product product);
        Task<bool> DeleteProductAsync(int id);

        // Additional functionality
        Task<IEnumerable<Product>> GetActiveProductsAsync();
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(string category);
        Task<Product> GetProductByBarcodeAsync(string barcode);

        // For pagination and filtering
        Task<(IEnumerable<Product> products, int totalCount)> GetProductsPagedAsync(
            int pageNumber,
            int pageSize,
            string searchTerm = null,
            string category = null,
            bool? status = null);

        // Stock management
        Task<decimal> GetProductStockQuantityAsync(int productId);
        Task<bool> UpdateStockQuantityAsync(int productId, decimal quantityChange);
    }
}
