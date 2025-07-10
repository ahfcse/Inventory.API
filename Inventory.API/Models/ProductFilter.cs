namespace Inventory.API.Models
{
    public class ProductFilter
    {
        public string Name { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string SearchTerm { get; set; }
        public string Category { get; set; }
        public bool? Status { get; set; }
        public string SortBy { get; set; } = "Name";
        public bool SortDescending { get; set; } = false;
    }
}
