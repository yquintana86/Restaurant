using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace Restaurant.Infrastructure.Configurations
{
    public class WaiterConfiguration : IEntityTypeConfiguration<Waiter>
    {
        public void Configure(EntityTypeBuilder<Waiter> builder)
        {
            //Waiter - Reservation Relationship
            builder.HasMany(w => w.Reservations)                
             .WithOne(w => w.Waiter)
             .HasForeignKey(w => w.WaiterId)
             .IsRequired()
             .OnDelete(DeleteBehavior.NoAction);

            //Waiter - WorkHistory RelationShip
            builder.HasMany(w => w.WorkHistories)
                .WithOne(wh => wh.Waiter);

            builder.Property(w => w.FirstName).HasColumnType("nvarchar(50)");
            builder.Property(w => w.LastName).HasColumnType("nvarchar(150)");
            builder.Property(w => w.Salary).HasColumnType("decimal(7,2)");
            builder.Property(w => w.Start).HasDefaultValue(DateTime.Now);
            builder.Ignore(w => w.GetFullName);
        }
    }
}
