using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ResturantManagerMVC.Models;
using System.Reflection.Emit;

namespace ResturantManagerMVC.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<OrderDish> OrderDishes { get; set; }
        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Order>()
                .HasOne(c => c.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey("CustomerId");

            builder.Entity<OrderDish>()
           .HasKey(od => new { od.OrderId, od.DishId });

            builder.Entity<OrderDish>()
                .HasOne(od => od.Order)
                .WithMany(o => o.OrderDishes)
                .HasForeignKey(od => od.OrderId);

            builder.Entity<OrderDish>()
                .HasOne(od => od.Dish)
                .WithMany(d => d.OrderDishes)
                .HasForeignKey(od => od.DishId);
            builder.Entity<Dish>()
                .Property(d => d.Price)
                .HasPrecision(18, 2); 
        }
    }
}
