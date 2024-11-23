namespace Heritage.Application.DataTransferObjects;

public record CreateRoomDto
{
    public int Id { get; set; }
    public string Image { get; set; } = null!;
    public decimal Price { get; set; }
}
