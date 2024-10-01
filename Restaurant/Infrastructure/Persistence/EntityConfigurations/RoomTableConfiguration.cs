using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedLib;
using System.Diagnostics.CodeAnalysis;


namespace Infrastructure.Persistence.EntityConfigurations
{
    public class RoomTableConfiguration : IEntityTypeConfiguration<RoomTable>
    {
        public void Configure(EntityTypeBuilder<RoomTable> builder)
        {
            builder.HasKey(t => new { t.RoomId, t.Id });

            builder.Property(rt => rt.Id).UseIdentityColumn();

            builder.HasOne(t => t.Waiter)
                     .WithMany(w => w.RoomTables)
                     .HasForeignKey(t => t.WaiterId)
                     .IsRequired(false)
                     .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasMany(t => t.Dishes)
            .WithMany(d => d.Tables)
            .UsingEntity<TableDish>();

            builder.HasMany(t => t.Reservations)
            .WithMany(r => r.Tables);           

            builder.Property(t => t.Status)
            .HasConversion(t => t.ToString(), t => (RoomTableStatusType)Enum.Parse(typeof(RoomTableStatusType), t))
            .HasMaxLength(10)
            .IsUnicode(false); //Also can be:
                               //t.Property(t => t.Status).HasConversion<string>();  and the exact built in ValueConverter class will be instantiated
                               //t.Property(t => t.Status).HasColumnType("nvarchar(10)"); also the compiler will do the same as above.
                               //Also can create a instance of the ValueConverter<Expression<Func<TableStatus,string>>,Expression<Func<string,TableStatus>>> class and passed as a parameter to HasConvertion Method.
        }

    }
}
