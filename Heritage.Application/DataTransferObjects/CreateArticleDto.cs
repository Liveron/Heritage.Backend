using System.ComponentModel.DataAnnotations;

namespace Heritage.Application.DataTransferObjects;

public record CreateArticleDto
{
    [Required(ErrorMessage = "Article title is required.")]
    public string Title { get; init; } = null!;

    [Required(ErrorMessage = "Content of article is required.")]
    public string Content { get; init; } = null!;

    [Required(ErrorMessage = "Article preview is required.")]
    public CreatePreviewDto Preview { get; init; } = null!;
}
