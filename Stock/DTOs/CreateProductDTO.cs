namespace Stock.Models
{
    public class CreateProductDTO
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
