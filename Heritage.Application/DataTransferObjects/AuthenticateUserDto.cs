using System.ComponentModel.DataAnnotations;

namespace Heritage.Application.DataTransferObjects;

public record AuthenticateUserDto
{
    [Required(ErrorMessage = "User name for authentication is required.")]
    public string UserName { get; init; } = null!;
    [Required(ErrorMessage = "Password for authentication is requird.")]
    public string Password { get; init; } = null!;
}
