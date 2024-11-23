using System.ComponentModel.DataAnnotations;

namespace Heritage.Application.DataTransferObjects;

public record RegisterUserDto
{
    [Required(ErrorMessage = "First name for registration is required.")]
    public string FirstName { get; init; } = null!;
    [Required(ErrorMessage = "Last name for registration is required.")]
    public string LastName { get; init; } = null!;
    [Required(ErrorMessage = "Username for registration is required.")]
    public string UserName { get; init; } = null!;
    [Required(ErrorMessage = "Password is required.")]
    public string Password { get; init; } = null!;
    [Required(ErrorMessage = "Email is required.")]
    public string? Email { get; init; }
    public string? PhoneNumber { get; init; }
    public ICollection<string> Roles { get; init; } = [];
}
