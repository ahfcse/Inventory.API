namespace Inventory.API.Models
{
    public class Sale
    {
        public int SaleId { get; set; }
        public DateTime SaleDate { get; set; }
        public int? CustomerId { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal DueAmount { get; set; }
        public Customer Customer { get; set; }
        public List<SaleDetail> SaleDetails { get; set; }
    }
}
