namespace Stock.Models
{
    public class ProductTests
    {
        public int Id { get; set; }
        
        public required string Name { get; set; }
        public required string Description { get; set; }
        
         public int Quantity { get; set; }
        public decimal Price { get; set; }
        
        public DateTime DateAdded { get; set; } = DateTime.UtcNow; 
        public DateTime DateUpdated { get; set; } = DateTime.UtcNow; 
        
        public ProductTests()
        {
            DateUpdated = DateTime.UtcNow; 
        }
    }
}