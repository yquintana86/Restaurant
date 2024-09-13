using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedLib;
using System.Diagnostics.CodeAnalysis;


namespace Infrastructure.Persistence.EntityConfigurations
{
    public class TableConfiguration : IEntityTypeConfiguration<Table>
    {
        public void Configure(EntityTypeBuilder<Table> builder)
        {
            builder.HasKey(t => new { t.RoomId, t.Id });

            builder.HasOne(t => t.Waiter)
                     .WithMany(w => w.Tables)
                     .HasForeignKey(t => t.WaiterId)
                     .IsRequired(false);

            builder.HasMany(t => t.Dishes)
            .WithMany(d => d.Tables)
            .UsingEntity<TableDish>();

            builder.HasMany(t => t.Reservations)
            .WithMany(r => r.Tables)
            .UsingEntity(
                "Reservation_Table",
                r => r.HasOne(typeof(Reservation)).WithMany().HasForeignKey("reservation_id").HasPrincipalKey(nameof(Reservation.Id)),
                t => t.HasOne(typeof(Table)).WithMany().HasForeignKey("room_id", "table_id").HasPrincipalKey(nameof(Table.RoomId), nameof(Table.Id)),
                rt => rt.HasKey("room_id", "table_id", "reservation_id"));

            builder.Property(t => t.Status)
            .HasConversion(t => t.ToString(), t => (TableStatusType)Enum.Parse(typeof(TableStatusType), t))
            .HasMaxLength(10)
            .IsUnicode(false); //Also can be:
                               //t.Property(t => t.Status).HasConversion<string>();  and the exact built in ValueConverter class will be instantiated
                               //t.Property(t => t.Status).HasColumnType("nvarchar(10)"); also the compiler will do the same as above.
                               //Also can create a instance of the ValueConverter<Expression<Func<TableStatus,string>>,Expression<Func<string,TableStatus>>> class and passed as a parameter to HasConvertion Method.
        }

    }
}
