using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Infrastructure.Persistence.EntityConfigurations;

public class RoomConfiguration : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.HasOne(rm => rm.Waiter)
                 .WithOne(w => w.Room)
                 .HasForeignKey<Room>(rm => rm.WaiterId)
                 .IsRequired();

        builder.HasMany(rm => rm.Tables)
        .WithOne(t => t.Room)
        .HasForeignKey(t => t.RoomId)
        .IsRequired(false);

        builder.Property(r => r.Name).HasColumnType("nvarchar(50)");
    }
}
