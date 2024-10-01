namespace Domain.Entities;

public class Room
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Theme { get; set; }
    public string? Description { get; set; }


    #region Navigation Properties

    //Room-Waiter FK and reference Navigation on required relationship 
    public int WaiterId { get; set; }
    public Waiter Waiter { get; set; } = null!;

    //Room-Table collection navigation of the relationship
    public ICollection<RoomTable>? Tables { get; init; } = new List<RoomTable>();

    #endregion
}
