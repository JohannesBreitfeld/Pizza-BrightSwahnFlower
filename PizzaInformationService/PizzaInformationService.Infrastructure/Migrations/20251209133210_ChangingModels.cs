using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PizzaInformationService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangingModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Pizzas");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Pizzas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Pizzas",
                keyColumn: "Id",
                keyValue: 1,
                column: "Description",
                value: "Classic pizza with tomato sauce, mozzarella cheese, and fresh basil.");

            migrationBuilder.UpdateData(
                table: "Pizzas",
                keyColumn: "Id",
                keyValue: 2,
                column: "Description",
                value: "Tomato sauce, mozzarella cheese, and pepperoni slices.");

            migrationBuilder.UpdateData(
                table: "Pizzas",
                keyColumn: "Id",
                keyValue: 3,
                column: "Description",
                value: "BBQ sauce, grilled chicken, red onions, and cilantro.");

            migrationBuilder.UpdateData(
                table: "Pizzas",
                keyColumn: "Id",
                keyValue: 4,
                column: "Description",
                value: "Unique pizza topped with kangaroo meat, bush tomatoes, and native herbs.");
        }
    }
}
