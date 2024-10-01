using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.DataContext
{
    internal sealed class DbInitializer
    {
        private readonly ModelBuilder _modelBuilder;
        public DbInitializer(ModelBuilder modelBuilder) 
        {
            _modelBuilder = modelBuilder;
        }

        public void SeedData()
        {
            _modelBuilder.Entity<Waiter>().HasData(
                new Waiter { Id = 8, FirstName = "John", LastName = "Doe", Salary = 3000.00M },
                new Waiter { Id = 12, FirstName = "Jane", LastName = "Doe", Salary = 5000.00M },
                new Waiter { Id = 13, FirstName = "Smith", LastName = "Johnson", Salary = 6000.00M });

            _modelBuilder.Entity<Room>().HasData(
                new Room { Id = 7, Name="Italian", WaiterId = 8, Theme = "Italian Menu", Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It h" },
                new Room { Id = 8, Name = "Caribean", WaiterId = 12, Theme = "Caribean Food", Description = "It is a long established fact that a reader will be distracted by the readable content of a page when looking at its layout. The point of using Lorem Ipsum is that it has a more-or-less normal distribution of letters, as opposed to using 'Content her" });

            _modelBuilder.Entity<RoomTable>().HasData(
                new RoomTable() { Id = 2, RoomId = 7, TotalQty = 2, WaiterId = 8, Status = SharedLib.RoomTableStatusType.Reserved },
                new RoomTable() { Id = 3, RoomId = 7, TotalQty = 2, WaiterId = 12, Status = SharedLib.RoomTableStatusType.Busy },
                new RoomTable() { Id = 4, RoomId = 8, TotalQty = 2, WaiterId = 8, Status = SharedLib.RoomTableStatusType.Unreserved },
                new RoomTable() { Id = 5, RoomId = 8, TotalQty = 4, WaiterId = 13, Status = SharedLib.RoomTableStatusType.Reserved },
                new RoomTable() { Id = 6, RoomId = 8, TotalQty = 6, WaiterId = 13, Status = SharedLib.RoomTableStatusType.Unreserved }
                );
        }
    }
}
