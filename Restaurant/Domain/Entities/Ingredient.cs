
namespace Domain.Entities;

public class Ingredient
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public decimal Price { get; set; }

    #region Navigation Properties
    public ICollection<Dish> Dishes { get; init; } = new List<Dish>();
    public ICollection<DishIngredient> DishIngredient { get; init; } = new List<DishIngredient>();

    #endregion
}
