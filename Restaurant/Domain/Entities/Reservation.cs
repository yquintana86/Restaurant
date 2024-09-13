namespace Domain.Entities;

public class Reservation
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int DinersQty { get; set; }


    #region Navigation Properties

    //Client
    public int ClientId { get; set; }
    public Client Client { get; set; } = null!;

    //Shift
    public int ShiftId { get; set; }
    public Shift Shift { get; set; } = null!;

    //Captain(Waiter)
    public int WaiterId { get; set; }
    public Waiter Waiter { get; set; } = null!;

    //Table
    public ICollection<Table> Tables { get; } = new List<Table>();

    #endregion
}
