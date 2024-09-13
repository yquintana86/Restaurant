using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;


namespace Infrastructure.Persistence.EntityConfigurations;

public class TableDishConfiguration : IEntityTypeConfiguration<TableDish>
{
    public void Configure(EntityTypeBuilder<TableDish> builder)
    {
        builder.HasKey(td => new { td.TableId, td.RoomId, td.ShiftId, td.DishId, td.Date });

        builder.Property(td => td.Date).HasColumnType("date");
        builder.Property(td => td.Amount).HasColumnType("decimal(7,2)");
    }
}
