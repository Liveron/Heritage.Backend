using Heritage.Domain;
using Mapster;
using System.ComponentModel.DataAnnotations;

namespace Heritage.Application.DataTransferObjects;

public record UpdateRoomDto
{
    static UpdateRoomDto()
    {
        TypeAdapterConfig<UpdateRoomDto, Room>.NewConfig()
            .Map(dest => dest.Id, _ => (int)MapContext.Current!.Parameters["id"]);
    }

    [Required(ErrorMessage = "Room image is required.")]
    public string Image { get; set; } = null!;
    [Required(ErrorMessage = "Room price is required.")]
    public decimal Price { get; set; }
}
