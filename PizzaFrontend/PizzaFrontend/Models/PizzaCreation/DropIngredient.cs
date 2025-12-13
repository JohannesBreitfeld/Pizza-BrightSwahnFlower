namespace PizzaFrontend.Models.PizzaCreation
{
    public class DropIngredient
    {
        public string ImagePath => $"Images/PizzaCreationImages/{Ingredient.Name.Replace(" ", "")}.png";
        public string zIndex => Ingredient.Name == "Tomato Sauce" ? "100" : "101";
        public Ingredient Ingredient { get; set; }
        public string Zone { get; set; }
    }
}
