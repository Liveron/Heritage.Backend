using Heritage.Domain;
using Mapster;

namespace Heritage.Application.DataTransferObjects;

public class UpdateOrderDto
{
    public DateTime Start { get; init; }
    public DateTime End { get; init; }
    public int RoomId { get; init; }
    public string UserName { get; init; } = null!;
}
