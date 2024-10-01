using SharedLib;

namespace Domain.Entities;

public class Reservation
{
    public int Id { get; set; }
    public DateTime Start { get; set; }
    public DateTime? End { get; set; }
    public int DinersQty { get; set; }
    public ReservationStatusType Status { get; set; }


    #region Navigation Properties

    //Client
    public int ClientId { get; set; }
    public Client Client { get; set; } = null!;

    //Reservation    
    public ICollection<TableDish> TableDishes { get; init; } = new List<TableDish>();

    //Made it by Waiter
    public int WaiterId { get; set; }
    public Waiter Waiter { get; set; } = null!;

    //Table
    public ICollection<RoomTable> Tables { get; init; } = new List<RoomTable>();

    #endregion
}
