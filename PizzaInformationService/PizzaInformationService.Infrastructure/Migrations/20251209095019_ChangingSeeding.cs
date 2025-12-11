using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PizzaInformationService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangingSeeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Ingredients",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 8, "Kangaroo Meat" },
                    { 9, "Bush Tomatoes" },
                    { 10, "Native Herbs" }
                });

            migrationBuilder.InsertData(
                table: "Pizzas",
                columns: new[] { "Id", "Description", "ImageUrl", "Name", "Price" },
                values: new object[] { 4, "Unique pizza topped with kangaroo meat, bush tomatoes, and native herbs.", "https://k-roo.com.au/wp-content/uploads/2014/06/Kangaroo-Pizza.jpg", "Kangaroo Pizza", 18.00m });

            migrationBuilder.InsertData(
                table: "IngredientPizza",
                columns: new[] { "IngredientsId", "PizzaId" },
                values: new object[,]
                {
                    { 8, 4 },
                    { 9, 4 },
                    { 10, 4 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "IngredientPizza",
                keyColumns: new[] { "IngredientsId", "PizzaId" },
                keyValues: new object[] { 8, 4 });

            migrationBuilder.DeleteData(
                table: "IngredientPizza",
                keyColumns: new[] { "IngredientsId", "PizzaId" },
                keyValues: new object[] { 9, 4 });

            migrationBuilder.DeleteData(
                table: "IngredientPizza",
                keyColumns: new[] { "IngredientsId", "PizzaId" },
                keyValues: new object[] { 10, 4 });

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Pizzas",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
