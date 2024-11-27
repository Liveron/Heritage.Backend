using Heritage.Application.Models;
using Mapster;

namespace Heritage.Application.DataTransferObjects;

public record CreateArticlePreviewDto
{
    static CreateArticlePreviewDto()
    {
        TypeAdapterConfig<CreateArticlePreviewDto, ArticlePreview>.NewConfig()
            .Map(dest => dest.ArticleId, _ => (Guid)MapContext.Current!.Parameters["id"]);
    }

    public string Title { get; init; } = null!;
    public string Image { get; init; } = null!;
}
