namespace PizzaInformationService.Domain.Entities
{
    public class Ingredient
    {
        public int Id { get; set; }
        public required string Name { get; set; }

        public List<Pizza>? Pizza { get; set; } = new List<Pizza>();
    }
}
