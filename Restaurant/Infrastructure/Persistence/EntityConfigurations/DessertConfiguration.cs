using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.Persistence.EntityConfigurations;

public class DessertConfiguration : IEntityTypeConfiguration<Dessert>
{
    public void Configure(EntityTypeBuilder<Dessert> builder)
    {
        builder.Property(d => d.QualityReview).HasColumnType("decimal(2,1)");
    }
}
