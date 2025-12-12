namespace PizzaFrontend.Models.PizzaCreation
{
    public class DropIngredient
    {
        public string ImagePath => $"Images/PizzaCreationImages/{Ingredient.Name.Replace(" ", "")}.png";
        public Ingredient Ingredient { get; set; }
        public string Zone { get; set; }
    }
}
