using System.ComponentModel.DataAnnotations;

namespace Heritage.Application.DataTransferObjects;

public class CreatePreviewDto
{
    [Required(ErrorMessage = "Preview title is requred.")]
    public string Title { get; init; } = null!;

    [Required(ErrorMessage = "Preview image is required.")]
    public string Image { get; init; } = null!;
}
