using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

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
            //Seed Data
            _modelBuilder.Entity<Shift>().HasData(
                new Shift { Id = 1003, Name = "First", StartTime = new DateTime(2024, 01, 01, 08, 00, 00), EndTime = new DateTime(2024, 01, 01, 10, 00, 00) },
                new Shift { Id = 1004, Name = "Second", StartTime = new DateTime(2024, 01, 01, 10, 15, 00), EndTime = new DateTime(2024, 01, 01, 12, 15, 00) },
                new Shift { Id = 1005, Name = "Third", StartTime = new DateTime(2024, 01, 01, 12, 30, 00), EndTime = new DateTime(2024, 01, 01, 14, 30, 00) },
                new Shift { Id = 1008, Name = "Fourth", StartTime = new DateTime(2024, 01, 01, 14, 45, 00), EndTime = new DateTime(2024, 01, 01, 16, 45, 00) },
                new Shift { Id = 1009, Name = "Fifth", StartTime = new DateTime(2024, 01, 01, 17, 00, 00), EndTime = new DateTime(2024, 01, 01, 19, 00, 00) },
                new Shift { Id = 1010, Name = "Sixth", StartTime = new DateTime(2024, 01, 01, 19, 15, 00), EndTime = new DateTime(2024, 01, 01, 21, 15, 00) });


            _modelBuilder.Entity<Waiter>().HasData(
                new Waiter { Id = 8, FirstName = "John", LastName = "Doe", Salary = 3000.00M, ShiftId = 1003 },
                new Waiter { Id = 12, FirstName = "Jane", LastName = "Doe", Salary = 5000.00M, ShiftId = 1004 },
                new Waiter { Id = 13, FirstName = "Smith", LastName = "Johnson", Salary = 6000.00M, ShiftId = 1005 });

            _modelBuilder.Entity<Room>().HasData(
                new Room { Id = 7, Name="Italian", WaiterId = 8, Theme = "Italian Menu", Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It h" },
                new Room { Id = 8, Name = "Caribean", WaiterId = 12, Theme = "Caribean Food", Description = "It is a long established fact that a reader will be distracted by the readable content of a page when looking at its layout. The point of using Lorem Ipsum is that it has a more-or-less normal distribution of letters, as opposed to using 'Content her" });
        }
    }
}
