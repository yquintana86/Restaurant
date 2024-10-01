using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;


namespace Infrastructure.Persistence.EntityConfigurations;

public class DishConfiguration : IEntityTypeConfiguration<Dish>
{
    public void Configure(EntityTypeBuilder<Dish> builder)
    {
        builder.Property(d => d.Price).HasColumnType("decimal(7,2)");
        builder.HasDiscriminator<string>("Dish_Discriminator")
                .HasValue<Dish>("Dish")
                .HasValue<Dessert>("Dessert")
                .HasValue<MainCourse>("MainCourse");

        builder.HasMany(d => d.Ingredients)
        .WithMany(i => i.Dishes)
        .UsingEntity<DishIngredient>();

        builder.Property(d => d.FirstServed).HasColumnType("DateTime");
    }
}
