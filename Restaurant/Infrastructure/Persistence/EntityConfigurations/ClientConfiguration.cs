using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.Persistence.EntityConfigurations
{
    public class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            //Primary Key
            builder.HasKey(c => c.Id);          //It is discovered by convention but is better to have it here

            //Client-Reservation Relationship 
            builder.HasMany(c => c.Reservations)
             .WithOne(r => r.Client)
             .HasForeignKey(r => r.ClientId)    //Even when this FK is achieve by convention is best to configured by Fluent API
             .OnDelete(DeleteBehavior.NoAction)
             .IsRequired();

            builder.Property(c => c.FirstName).HasColumnType("nvarchar(11)").IsRequired();
            builder.Property(c => c.LastName).HasColumnType("nvarchar(15)").IsRequired();
            builder.Property(c => c.Phone).HasColumnType("nvarchar(12)").IsRequired(false);
            builder.Property(c => c.Address).HasColumnType("nvarchar(250)").IsRequired(false);
            builder.Property(c => c.Mail).HasColumnType("nvarchar(250)").IsRequired(false);

            builder.ToTable(c => c.HasCheckConstraint("Chk_Phone_Mail", "Mail IS NOT NULL OR Phone IS NOT NULL"));
        }
    }
}
