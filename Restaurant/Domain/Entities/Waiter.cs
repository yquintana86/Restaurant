namespace Domain.Entities;

public class Waiter
{
    public int Id { get; set; } // Should be Unique by using Fluent API
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public decimal Salary { get; set; }
    public DateTime Start { get; set; }
    public DateTime? End { get; set; }

    public string GetFullName { 
        get
        {
            return $"{FirstName} {LastName}";
        }
    }

    #region Navigation Properties

    //Waiter - Room reference navigation
    public Room? Room { get; set; }

    //Waiter-Table collection reference navigation
    public ICollection<RoomTable> RoomTables { get; init; } = new List<RoomTable>();

    //Waiter - Reservation Collection reference relationship
    public ICollection<Reservation> Reservations { get; init; } = new List<Reservation>();

    //Waiter Work History Collection Reference relationShip
    public ICollection<WorkHistory> WorkHistories { get; init; } = new List<WorkHistory>();

    #endregion
}
