using Inventory.API.Models;

namespace Inventory.API.Interfaces
{
    public interface ISaleService
    {
        // Sale operations
        Task<Sale> GetSaleByIdAsync(int id);
        Task<IEnumerable<Sale>> GetAllSalesAsync();
        Task<Sale> CreateSaleAsync(Sale sale);

        // Reporting
        Task<SalesReport> GetSalesReportAsync(DateTime fromDate, DateTime toDate);
        Task<IEnumerable<Sale>> GetSalesByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Sale>> GetSalesByCustomerAsync(int customerId);

        // Financials
        Task<decimal> GetTotalRevenueAsync(DateTime? fromDate = null, DateTime? toDate = null);
        Task<int> GetTotalTransactionsAsync(DateTime? fromDate = null, DateTime? toDate = null);

        // Inventory impact
        Task<bool> ProcessSaleInventoryImpactAsync(int saleId);

        // Customer loyalty
        Task<int> CalculateLoyaltyPointsAsync(int saleId);
        Task<bool> ApplyLoyaltyPointsAsync(int customerId, int pointsToRedeem);
    }
}
