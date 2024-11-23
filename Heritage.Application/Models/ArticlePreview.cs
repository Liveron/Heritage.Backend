using System.ComponentModel.DataAnnotations;

namespace Heritage.Application.Models;

public class ArticlePreview
{
    public Guid ArticleId { get; init; }

    [Required(ErrorMessage = "Article preview title is required.")]
    public string Title { get; init; } = null!;

    [Required(ErrorMessage = "Article image is required.")]
    public string Image { get; init; } = null!;
}
