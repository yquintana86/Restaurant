namespace Domain.Entities;

public class Client
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? Phone { get; set; }      //A validation is required "Phone or mail must have a value";
    public string? Mail { get; set; }       //A validation is required "Phone or mail must have a value";
    public string? Address { get; set; }
    //Reservation
    public ICollection<Reservation> Reservations { get; } = new List<Reservation>();
}
