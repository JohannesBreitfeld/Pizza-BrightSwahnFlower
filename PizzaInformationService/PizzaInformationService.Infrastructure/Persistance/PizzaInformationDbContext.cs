using Microsoft.EntityFrameworkCore;
using PizzaInformationService.Application.Abstractions;
using PizzaInformationService.Domain.Entities;
using System.Reflection.Metadata;

namespace PizzaInformationService.Infrastructure.Persistance
{
    public class PizzaInformationDbContext : DbContext, IPizzaInformationDbContext
    {
        public DbSet<Pizza> Pizzas { get; set; } = null!;
        public DbSet<Ingredient> Ingredients { get; set; } = null!;


        IQueryable<Pizza> IPizzaInformationDbContext.Pizzas => Pizzas;
        IQueryable<Ingredient> IPizzaInformationDbContext.Ingredients => Ingredients;

        public PizzaInformationDbContext(DbContextOptions<PizzaInformationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure Price property precision
            modelBuilder.Entity<Pizza>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Pizza>().HasData(     
                new Pizza { Id = 1, Name = "Margherita", Price = 13.00M, ImageUrl = "https://images.unsplash.com/photo-1574071318508-1cdbab80d002?q=80&w=1469&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D" },
                new Pizza { Id = 2, Name = "Pepperoni Pizza", Price = 14.00M, ImageUrl = "https://images.unsplash.com/photo-1534308983496-4fabb1a015ee?q=80&w=1476&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D" },
                new Pizza { Id = 3, Name = "BBQ Chicken", Price = 15.00M, ImageUrl = "https://images.unsplash.com/photo-1565299624946-b28f40a0ae38?q=80&w=781&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D" },
                new Pizza { Id = 4, Name = "Kangaroo Pizza", Price = 18.00M, ImageUrl = "https://k-roo.com.au/wp-content/uploads/2014/06/Kangaroo-Pizza.jpg" } 
            );

            modelBuilder.Entity<Ingredient>().HasData(
                new Ingredient { Id = 1, Name = "Tomato Sauce" },
                new Ingredient { Id = 2, Name = "Mozzarella Cheese" },
                new Ingredient { Id = 3, Name = "Fresh Basil" },
                new Ingredient { Id = 4, Name = "Pepperoni" },
                new Ingredient { Id = 5, Name = "Grilled Chicken" },
                new Ingredient { Id = 6, Name = "Red Onions" },
                new Ingredient { Id = 7, Name = "Cilantro" },
                new Ingredient { Id = 8, Name = "Kangaroo Meat" },
                new Ingredient { Id = 9, Name = "Bush Tomatoes" },
                new Ingredient { Id = 10, Name = "Native Herbs" }
            );

            modelBuilder.Entity<Pizza>()
                .HasMany(p => p.Ingredients)
                .WithMany(i => i.Pizza)
                .UsingEntity(j => j.HasData(
                    new { PizzaId = 1, IngredientsId = 1 },
                    new { PizzaId = 1, IngredientsId = 2 },
                    new { PizzaId = 1, IngredientsId = 3 },
                    new { PizzaId = 2, IngredientsId = 1 },
                    new { PizzaId = 2, IngredientsId = 2 },
                    new { PizzaId = 2, IngredientsId = 4 },
                    new { PizzaId = 3, IngredientsId = 5 },
                    new { PizzaId = 3, IngredientsId = 6 },
                    new { PizzaId = 3, IngredientsId = 7 },
                    new { PizzaId = 4, IngredientsId = 8 },
                    new { PizzaId = 4, IngredientsId = 9 },
                    new { PizzaId = 4, IngredientsId = 10 }
                ));

            base.OnModelCreating(modelBuilder);
        }

    }
}
