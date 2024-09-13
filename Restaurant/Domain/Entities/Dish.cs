namespace Domain.Entities;

public class Dish
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public DateTime Begin { get; set; }
    public decimal Price { get; set; }

    #region Navigation Properties

    //Table-Table_Dish-Dish Relationship with join table with payload
    public ICollection<Table> Tables { get; } = new List<Table>();
    public ICollection<TableDish> TablesDishes { get; } = new List<TableDish>();


    //Dish-Dish_Ingredient-Ingredient relationship with a join table with payload
    public ICollection<Ingredient> Ingredients { get; } = new List<Ingredient>();
    public ICollection<DishIngredient> DishIngredient { get; } = new List<DishIngredient>();

    #endregion
}
