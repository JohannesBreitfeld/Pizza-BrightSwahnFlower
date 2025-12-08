namespace PizzaFrontend.Contracts.Pizzas
{
    public class Pizza
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsSelected { get; set; }
        public List<Ingredient> Ingredients { get; set; } = [];
    }
}
