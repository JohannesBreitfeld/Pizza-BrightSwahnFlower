namespace PizzaInformationService.Domain.Entities
{
    public class Pizza
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public List<Ingredient>? Ingredients { get; set; } = new List<Ingredient>();

    }
}
