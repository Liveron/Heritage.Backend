using System.ComponentModel.DataAnnotations;

namespace Heritage.Application.Models;

public class Article
{
    public Guid Id { get; init; } = Guid.NewGuid();

    [Required(ErrorMessage = "Article title is required.")]
    public string Title { get; init; } = null!;

    [Required(ErrorMessage = "Article content is required.")]
    public string Content { get; init; } = null!;
    public ArticlePreview? Preview { get; init; }
}
