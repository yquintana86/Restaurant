namespace Domain.Entities;

public class Dish
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public DateTime? FirstServed { get; set; }
    public decimal Price { get; set; }

    #region Navigation Properties

    //Table-Table_Dish-Dish Relationship with join table with payload
    public ICollection<RoomTable> Tables { get; init; } = new List<RoomTable>();
    public ICollection<TableDish> TablesDishes { get; init; } = new List<TableDish>();


    //Dish-Dish_Ingredient-Ingredient relationship with a join table with payload
    public ICollection<Ingredient> Ingredients { get; init; } = new List<Ingredient>();
    public ICollection<DishIngredient> DishIngredient { get; init; } = new List<DishIngredient>();

    #endregion
}
