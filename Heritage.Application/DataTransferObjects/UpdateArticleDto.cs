using Heritage.Application.Models;
using Mapster;

namespace Heritage.Application.DataTransferObjects;

public record UpdateArticleDto
{
    static UpdateArticleDto()
    {
        TypeAdapterConfig<UpdateArticleDto, Article>.NewConfig()
            .Map(dest => dest.Id, _ => (Guid)MapContext.Current!.Parameters["id"]);
    }

    public string Title { get; init; } = null!;
    public string Content { get; init; } = null!;
    public UpdateArticlePreviewDto Preview { get; init; } = null!;
}