using Application.Abstractions.Data;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Infrastructure.Persistence.DataContext
{
    public class EFCoreDbContext : DbContext, IEFCoreDbContext
    {

        public EFCoreDbContext()
        {

        }

        public EFCoreDbContext(DbContextOptions<EFCoreDbContext> dbContextOptions) :
            base(dbContextOptions)
        { }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Waiter> Waiters { get; set; }
        public DbSet<WorkHistory> WorkHistory { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomTable> RoomTables { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<TableDish> TableDishes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<DishIngredient> DishIngredient { get; set; }
        public DbSet<Dessert> Desserts { get; set; }
        public DbSet<MainCourse> MainCourses { get; set; }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            new DbInitializer(modelBuilder).SeedData();
        }

    }
}
