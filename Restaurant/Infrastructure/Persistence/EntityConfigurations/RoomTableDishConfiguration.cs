using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;


namespace Infrastructure.Persistence.EntityConfigurations;

public class RoomTableDishConfiguration : IEntityTypeConfiguration<TableDish>
{
    public void Configure(EntityTypeBuilder<TableDish> builder)
    {
        builder.HasKey(td => new { td.TableId, td.TableRoomId, td.ReservationId, td.DishId });

        builder.Property(td => td.Amount).HasColumnType("decimal(7,2)");
    }
}
