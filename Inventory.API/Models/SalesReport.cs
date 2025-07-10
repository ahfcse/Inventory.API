namespace Inventory.API.Models
{
    public class SalesReport
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TotalSales { get; set; }
        public decimal TotalRevenue { get; set; }
        public int NumberOfTransactions { get; set; }
        public decimal AverageSaleAmount { get; set; }
        public Dictionary<string, decimal> RevenueByCategory { get; set; }
    }
}
