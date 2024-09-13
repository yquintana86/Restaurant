using SharedLib;

namespace Domain.Entities;

public class Table
{
    public int Id { get; set; }
    public TableStatusType Status { get; set; }
    public int TotalQty { get; set; }


    #region Navigation Properties

    //Table-Room required relationship FK and reference navigation

    public int RoomId { get; set; }     //This is part of the composite key(Id,RoomId) for this entity
    public Room Room { get; set; } = null!;


    //Table-Room required relationship FK and reference navigation

    public int? WaiterId { get; set; }
    public Waiter? Waiter { get; set; } = null!;


    //Table-Reservation
    public ICollection<Reservation> Reservations { get; } = new List<Reservation>();

    //Table-Table_Dish
    public ICollection<Dish> Dishes { get; } = new List<Dish>();
    public ICollection<TableDish> TablesDishes { get; } = new List<TableDish>();


    #endregion
}
