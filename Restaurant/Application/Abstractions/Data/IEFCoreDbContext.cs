using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;


namespace Application.Abstractions.Data;

public interface IEFCoreDbContext
{
    DbSet<Client> Clients { get; set; }
    DbSet<Reservation> Reservations { get; set; }
    DbSet<Waiter> Waiters { get; set; }
    DbSet<WorkHistory> WorkHistory { get; set; }
    DbSet<Room> Rooms { get; set; }
    DbSet<Domain.Entities.RoomTable> RoomTables { get; set; }
    DbSet<Dish> Dishes { get; set; }
    DbSet<TableDish> TableDishes { get; set; }
    DbSet<Ingredient> Ingredients { get; set; }
    DbSet<DishIngredient> DishIngredient { get; set; }
    DbSet<Dessert> Desserts { get; set; }
    DbSet<MainCourse> MainCourses { get; set; }
    DatabaseFacade Database { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}


