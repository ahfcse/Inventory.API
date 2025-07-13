using Inventory.API.Exceptions;
using Inventory.API.Interfaces;
using Inventory.API.Models;
using Microsoft.Extensions.Logging;

namespace Inventory.API.Repositories
{
    // ProductService.cs
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ProductService> _logger;
        public ProductService(IUnitOfWork unitOfWork, ILogger<ProductService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Product> AddProductAsync(Product product)
        {
            // Check for unique barcode using repository
            var existingProduct = (await _unitOfWork.Products.FindAsync(p => p.Barcode == product.Barcode)).FirstOrDefault();
            if (existingProduct != null)
                throw new Exception("Barcode must be unique");

            await _unitOfWork.Products.AddAsync(product);
            await _unitOfWork.CompleteAsync();
            return product;
        }

        public async Task<Product> GetProductByIdAsync(int id)
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

        public async Task<bool> DeleteProductAsync(int id)
        {
            // Validate input
            if (id <= 0)
            {
                throw new ArgumentException("Product ID must be greater than zero", nameof(id));
            }

            try
            {
                // Get existing product
                var product = await _unitOfWork.Products.GetByIdAsync(id);

                // Check if product exists and isn't already deleted
                if (product == null || product.IsDeleted)
                {
                    _logger.LogWarning("Delete failed - Product {ProductId} not found or already deleted", id);
                    return false;
                }

                // Soft delete implementation
                product.IsDeleted = true;
                product.Status = false; // Optionally mark as inactive
                _unitOfWork.Products.Update(product);

                // Commit changes
                int affectedRows = await _unitOfWork.CompleteAsync();

                if (affectedRows > 0)
                {
                    _logger.LogInformation("Successfully soft-deleted product {ProductId}", id);
                    return true;
                }

                _logger.LogWarning("No rows affected when deleting product {ProductId}", id);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting product {ProductId}", id);
                throw; // Re-throw for global exception handling
            }
        }
    

        Task<IEnumerable<Product>> IProductService.GetActiveProductsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            var query = await _unitOfWork.Products.GetAllAsync();
            return query.ToList();
        }

        Task<Product> IProductService.GetProductByBarcodeAsync(string barcode)
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

        public async Task<Product> UpdateProductAsync(int id, Product product)
        {
            // Validate input
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            if (id != product.ProductId)
                throw new ArgumentException("Product ID mismatch");

            // Get existing product
            var existingProduct = await _unitOfWork.Products.GetByIdAsync(id);
            if (existingProduct == null || existingProduct.IsDeleted)
                throw new ProductNotFoundException(id);

            // Validate barcode uniqueness if changed
            if (existingProduct.Barcode != product.Barcode)
            {
                var productWithSameBarcode = (await _unitOfWork.Products.FindAsync(
                    p => p.Barcode == product.Barcode && !p.IsDeleted))
                    .FirstOrDefault();

                if (productWithSameBarcode != null && productWithSameBarcode.ProductId != id)
                    throw new DuplicateBarcodeException(product.Barcode);
            }

            try
            {
                // Update product properties
                existingProduct.Name = product.Name;
                existingProduct.Barcode = product.Barcode;
                existingProduct.Price = product.Price;
                existingProduct.StockQty = product.StockQty;
                existingProduct.Category = product.Category;
                existingProduct.Status = product.Status;
                existingProduct.IsDeleted = product.IsDeleted;

                _unitOfWork.Products.Update(existingProduct);
                await _unitOfWork.CompleteAsync();

                _logger.LogInformation("Product {ProductId} updated successfully", id);
                return existingProduct;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating product {ProductId}", id);
                throw; // Re-throw for global exception handling
            }
        }

        // ... other methods ...
    

        Task<bool> IProductService.UpdateStockQuantityAsync(int productId, decimal quantityChange)
        {
            throw new NotImplementedException();
        }

        // Other CRUD operations...
    }
}
