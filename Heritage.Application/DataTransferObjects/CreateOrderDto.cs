using Heritage.Application.Models;
using Heritage.Domain;
using Mapster;

namespace Heritage.Application.DataTransferObjects;

public record CreateOrderDto
{
    static CreateOrderDto()
    {
        TypeAdapterConfig<CreateOrderDto, Order>.NewConfig()
            .Map(dest => dest.UserId, _ => (string)MapContext.Current!.Parameters["id"]);
    }

    public int RoomId { get; init; }
    public DateTime Start { get; init; }
    public DateTime End { get; set; }
}
