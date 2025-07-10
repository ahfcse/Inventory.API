namespace Inventory.API.Models
{
    public abstract class BaseEntity
    {
        public bool IsDeleted { get; set; } = false;
    }
}
