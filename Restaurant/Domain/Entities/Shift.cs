namespace Domain.Entities;

public class Shift
{
    public Shift()
    {
            
    }
    
    public Shift(string name, DateTime startTime, DateTime endTime)
    {
        Name = name;
        StartTime = startTime;
        EndTime = endTime;
    }

    public Shift(int id, string name, DateTime startTime, DateTime endTime)
    {
        Id = id;
        Name = name;
        StartTime = startTime;
        EndTime = endTime;
    }

    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    #region Navigation Properties

    public ICollection<Reservation> Reservations { get; init; } = new List<Reservation>();

    public ICollection<Waiter> Waiters { get; init; } = new List<Waiter>();

    public ICollection<TableDish> TableDishs { get; init; } = new List<TableDish>();

    #endregion
}

