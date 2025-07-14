using Inventory.API.Exceptions;
using Inventory.API.Interfaces;
using Inventory.API.Models;

namespace Inventory.API.Repositories
{
    // SaleService.cs
    public class SaleService : ISaleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private static readonly SemaphoreSlim _saleSemaphore = new SemaphoreSlim(3, 3);

        public SaleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Sale> CreateSaleAsync(Sale saleDto)
        {
            // Validate discount and VAT
            if (saleDto.DiscountPercentage < 0 || saleDto.DiscountPercentage > 100)
                throw new ArgumentException("Discount percentage must be between 0 and 100");

            if (saleDto.VatPercentage < 0 || saleDto.VatPercentage > 100)
                throw new ArgumentException("VAT percentage must be between 0 and 100");

            // Calculate subtotal from sale details
            var subTotal = saleDto.SaleDetails.Sum(d => d.Quantity * d.Price);

            // Calculate discount (prioritize percentage over fixed amount)
            decimal discountAmount = 0;
            if (saleDto.DiscountPercentage > 0)
            {
                discountAmount = subTotal * (saleDto.DiscountPercentage / 100);
            }
            else
            {
                discountAmount = saleDto.DiscountAmount;
            }

            // Calculate VAT on the discounted amount
            decimal vatAmount = (subTotal - discountAmount) * (saleDto.VatPercentage / 100);

            // Create sale entity
            var sale = new Sale
            {
                SaleDate = DateTime.UtcNow,
                CustomerId = saleDto.CustomerId,
                SubTotal = subTotal,
                DiscountPercentage = saleDto.DiscountPercentage,
                DiscountAmount = discountAmount,
                VatPercentage = saleDto.VatPercentage,
                VatAmount = vatAmount,
                TotalAmount = subTotal - discountAmount + vatAmount,
                PaidAmount = saleDto.PaidAmount,
                DueAmount = saleDto.DueAmount,
                SaleDetails = saleDto.SaleDetails.Select(d => new SaleDetail
                {
                    ProductId = d.ProductId,
                    Quantity = d.Quantity,
                    Price = d.Price
                }).ToList()
            };

            // Process sale (validate stock, save to DB, etc.)
            await ProcessSaleAsync(sale);

            return sale;
        }
        public async Task<Sale> ProcessSaleAsync(Sale sale)
        {
            if (!await _saleSemaphore.WaitAsync(0))// Limit to 3 concurrent
                throw new TooManyRequestsException();

            try
            {
                await Task.Delay(3000); // Simulate processing delay

                // Process each sale detail
                foreach (var detail in sale.SaleDetails)
                {
                    var product = await _unitOfWork.Products.GetByIdAsync(detail.ProductId);
                    if (product == null || product.StockQty < detail.Quantity)
                        throw new Exception($"Insufficient stock for product ID {detail.ProductId}");

                    product.StockQty -= detail.Quantity;
                    _unitOfWork.Products.Update(product);
                }

                // Calculate totals
                sale.TotalAmount = sale.SaleDetails.Sum(d => d.Quantity * d.Price);
                sale.DueAmount = sale.TotalAmount - sale.PaidAmount;
                sale.SaleDate = DateTime.UtcNow;

                await _unitOfWork.Sales.AddAsync(sale);
                await _unitOfWork.CompleteAsync();

                return sale;
            }
            finally
            {
                _saleSemaphore.Release();
            }
        }

        public async Task<SalesReport> GetSalesReportAsync(DateTime from, DateTime to)
        {
            var sales = await _unitOfWork.Sales.FindAsync(s =>
                s.SaleDate >= from && s.SaleDate <= to);

            return new SalesReport
            {
                TotalSales = sales.Count(),
                TotalRevenue = sales.Sum(s => s.TotalAmount),
                NumberOfTransactions = sales.Count()
            };
        }

        public async Task<Sale> GetSaleByIdAsync(int id)
        {
            var sales = await _unitOfWork.Sales.GetByIdAsync(id);
            return sales;
        }

        public async Task<IEnumerable<Sale>> GetAllSalesAsync()
        {
            var sales = await _unitOfWork.Sales.GetAllAsync();
            return sales;
        }

        Task<IEnumerable<Sale>> ISaleService.GetSalesByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Sale>> GetSalesByCustomerAsync(int customerId)
        {
            var sales = await _unitOfWork.Sales.GetSalesByCustomerAsync(customerId);
            return sales;
        }

        Task<decimal> ISaleService.GetTotalRevenueAsync(DateTime? fromDate, DateTime? toDate)
        {
            throw new NotImplementedException();
        }

        Task<int> ISaleService.GetTotalTransactionsAsync(DateTime? fromDate, DateTime? toDate)
        {
            throw new NotImplementedException();
        }

        Task<bool> ISaleService.ProcessSaleInventoryImpactAsync(int saleId)
        {
            throw new NotImplementedException();
        }

        Task<int> ISaleService.CalculateLoyaltyPointsAsync(int saleId)
        {
            throw new NotImplementedException();
        }

        Task<bool> ISaleService.ApplyLoyaltyPointsAsync(int customerId, int pointsToRedeem)
        {
            throw new NotImplementedException();
        }
    }
}
