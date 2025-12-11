using System.ComponentModel.DataAnnotations;

namespace PizzaInformationService.Application.Pizza.UpdatePizza
{
    public record UpdatePizzaRequest
    (
        [property: Required] string Name,
        [property: Required] string ImageUrl,
        [property: Range(0.01, double.MaxValue)] decimal Price,

        [property: Required] IEnumerable<int> IngredientIds
    );
}
