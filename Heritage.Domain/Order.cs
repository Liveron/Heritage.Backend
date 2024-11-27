namespace Heritage.Domain;

public class Order
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime Start { get; init; }
    public DateTime End { get; init; }
    public int RoomId { get; init; }
    public Room? Room { get; init; }
    public string UserId { get; init; } = null!;
    public User? User { get; set; }
}
