using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PizzaInformationService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSeeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Ingredients");

            migrationBuilder.InsertData(
                table: "Ingredients",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Tomato Sauce" },
                    { 2, "Mozzarella Cheese" },
                    { 3, "Fresh Basil" },
                    { 4, "Pepperoni" },
                    { 5, "Grilled Chicken" },
                    { 6, "Red Onions" },
                    { 7, "Cilantro" }
                });

            migrationBuilder.InsertData(
                table: "Pizzas",
                columns: new[] { "Id", "Description", "ImageUrl", "Name", "Price" },
                values: new object[,]
                {
                    { 1, "Classic pizza with tomato sauce, mozzarella cheese, and fresh basil.", "https://images.unsplash.com/photo-1574071318508-1cdbab80d002?q=80&w=1469&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", "Margherita", 13.00m },
                    { 2, "Tomato sauce, mozzarella cheese, and pepperoni slices.", "https://images.unsplash.com/photo-1534308983496-4fabb1a015ee?q=80&w=1476&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", "Pepperoni Pizza", 14.00m },
                    { 3, "BBQ sauce, grilled chicken, red onions, and cilantro.", "https://images.unsplash.com/photo-1565299624946-b28f40a0ae38?q=80&w=781&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", "BBQ Chicken", 15.00m }
                });

            migrationBuilder.InsertData(
                table: "IngredientPizza",
                columns: new[] { "IngredientsId", "PizzaId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 2 },
                    { 2, 1 },
                    { 2, 2 },
                    { 3, 1 },
                    { 4, 2 },
                    { 5, 3 },
                    { 6, 3 },
                    { 7, 3 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "IngredientPizza",
                keyColumns: new[] { "IngredientsId", "PizzaId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "IngredientPizza",
                keyColumns: new[] { "IngredientsId", "PizzaId" },
                keyValues: new object[] { 1, 2 });

            migrationBuilder.DeleteData(
                table: "IngredientPizza",
                keyColumns: new[] { "IngredientsId", "PizzaId" },
                keyValues: new object[] { 2, 1 });

            migrationBuilder.DeleteData(
                table: "IngredientPizza",
                keyColumns: new[] { "IngredientsId", "PizzaId" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.DeleteData(
                table: "IngredientPizza",
                keyColumns: new[] { "IngredientsId", "PizzaId" },
                keyValues: new object[] { 3, 1 });

            migrationBuilder.DeleteData(
                table: "IngredientPizza",
                keyColumns: new[] { "IngredientsId", "PizzaId" },
                keyValues: new object[] { 4, 2 });

            migrationBuilder.DeleteData(
                table: "IngredientPizza",
                keyColumns: new[] { "IngredientsId", "PizzaId" },
                keyValues: new object[] { 5, 3 });

            migrationBuilder.DeleteData(
                table: "IngredientPizza",
                keyColumns: new[] { "IngredientsId", "PizzaId" },
                keyValues: new object[] { 6, 3 });

            migrationBuilder.DeleteData(
                table: "IngredientPizza",
                keyColumns: new[] { "IngredientsId", "PizzaId" },
                keyValues: new object[] { 7, 3 });

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Pizzas",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Pizzas",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Pizzas",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Ingredients",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
