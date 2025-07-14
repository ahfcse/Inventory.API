using System.ComponentModel.DataAnnotations.Schema;

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
        public List<SaleDetail> SaleDetails { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal DiscountAmount { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal DiscountPercentage { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal VatAmount { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal VatPercentage { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal SubTotal { get; set; }

        // Calculated property for grand total
        [NotMapped]
        public decimal GrandTotal => SubTotal - DiscountAmount + VatAmount;
    }
}
