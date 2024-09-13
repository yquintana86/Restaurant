namespace Application.Shifts.Queries;

public sealed record ShiftResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public DateTime Begin { get; set; }
    public DateTime? End { get; set; }

}   



