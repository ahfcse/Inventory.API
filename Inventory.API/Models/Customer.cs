namespace Inventory.API.Models
{
    public class Customer:BaseEntity
    {
        public int CustomerId { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int LoyaltyPoints { get; set; }
       
    }
}
