namespace Domain.Entities;

public class WorkHistory
{
    public int Id { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }

    #region Navigation Properties
    public int WaiterId { get; set; }
    public Waiter Waiter { get; set; } = null!;

    #endregion
}
