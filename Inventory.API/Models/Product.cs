namespace Inventory.API.Models
{
    public class Product: BaseEntity
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Barcode { get; set; }
        public decimal Price { get; set; }
        public decimal StockQty { get; set; }
        public string Category { get; set; }
        public bool Status { get; set; }
      
    }
}
