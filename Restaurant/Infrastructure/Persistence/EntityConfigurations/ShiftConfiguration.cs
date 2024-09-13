using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;


namespace Infrastructure.Persistence.EntityConfigurations
{
    public class ShiftConfiguration : IEntityTypeConfiguration<Shift>
    {
        public void Configure(EntityTypeBuilder<Shift> builder)
        {
            //Shift Primary Key
            builder.HasKey(sh => sh.Id);

            //Shift-Reservation RelationShip
            builder.HasMany(sh => sh.Reservations)
              .WithOne(r => r.Shift)
              .HasForeignKey(r => r.ShiftId)
              .OnDelete(DeleteBehavior.NoAction)
              .IsRequired();

            //Shift-Waiter RelationShip
            builder.HasMany(sh => sh.Waiters)
              .WithOne(w => w.Shift)
              .HasForeignKey(w => w.ShiftId)
              .IsRequired();

            //Properties
            builder.Property(sh => sh.Name).IsRequired();
            builder.Property(sh => sh.StartTime).HasColumnType("datetime").IsRequired();
            builder.Property(sh => sh.EndTime).HasColumnType("datetime").IsRequired();
            builder.Property(sh => sh.Name).HasColumnType("nvarchar(15)").IsRequired();
        }
    }
}
