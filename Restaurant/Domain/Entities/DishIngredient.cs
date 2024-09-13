namespace Domain.Entities;

public class DishIngredient
{
    public int DishId { get; set; }
    public int IngredientId { get; set; }
    public int Quantity { get; set; }

    #region Reference Navigations
    public Dish Dish { get; set; } = null!;
    public Ingredient Ingredient { get; set; } = null!;

    #endregion
}
