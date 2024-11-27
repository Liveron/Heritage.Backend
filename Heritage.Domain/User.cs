using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Heritage.Domain;

public class User : IdentityUser
{
    [Required(ErrorMessage = "User first name is equired.")]
    public string FirstName { get; init; } = null!;

    [Required(ErrorMessage = "User last name is required.")]
    public string LastName { get; init; } = null!;
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
}
