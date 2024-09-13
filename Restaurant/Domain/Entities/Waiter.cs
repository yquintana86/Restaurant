using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Waiter
{
    public int Id { get; set; } // Should be Unique by using Fluent API
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public decimal Salary { get; set; }

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
    public ICollection<Table> Tables { get; } = new List<Table>();


    //Waiter - Reservation Collection reference relationship
    public ICollection<Reservation> Reservations { get; } = new List<Reservation>();

    //Waiter Shift FK and Reference Navigation
    public int ShiftId { get; set; }
    public Shift Shift { get; set; } = null!;

    #endregion
}
