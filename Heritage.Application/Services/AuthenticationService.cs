using Heritage.Application.Common.Exceptions;
using Heritage.Application.ConfigurationModels;
using Heritage.Application.DataTransferObjects;
using Heritage.Domain;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Heritage.Application.Services;

public interface IAuthenticationService
{
    Task<IdentityResult> RegisterUser(RegisterUserDto userDto);
    Task<bool> ValidateUser(AuthenticateUserDto userDto);
    Task<TokenDto> CreateToken(bool populateExp);
    Task<TokenDto> RefreshToken(TokenDto tokenDto);
}

public class AuthenticationService(UserManager<User> userManager, IOptions<JwtConfiguration> configuration) 
    : IAuthenticationService
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly JwtConfiguration _jwtConfiguration = configuration.Value;

    private User? _user;

    private readonly Lazy<JsonWebTokenHandler> _tokenHandler =
        new(() => new JsonWebTokenHandler());

    private JsonWebTokenHandler TokenHandler => _tokenHandler.Value;

    public async Task<IdentityResult> RegisterUser(RegisterUserDto userDto)
    {
        User user = userDto.Adapt<User>();

        var result = await _userManager.CreateAsync(user, userDto.Password);

        if (result.Succeeded)
            await _userManager.AddToRolesAsync(user, userDto.Roles);

        return result;
    }

    public async Task<bool> ValidateUser(AuthenticateUserDto userDto)
    {
        _user = await _userManager.FindByNameAsync(userDto.UserName);

        bool result = _user != null && await _userManager.CheckPasswordAsync(_user, userDto.Password);

        return result;
    }

    public async Task<TokenDto> CreateToken(bool populateExp)
    {
        if (_user == null)
            throw new InvalidOperationException("User is not set in authentication service.");

        SigningCredentials signingCredentials = GetSigningCredentials();
        Dictionary<string, object> claims = await GetClaims();
        SecurityTokenDescriptor tokenDescriptor = GenerateTokenDescriptor(signingCredentials, claims);
        string refreshToken = GenerateRefreshToken();

        _user.RefreshToken = refreshToken;

        if (populateExp)
            _user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7).ToUniversalTime();

        await _userManager.UpdateAsync(_user);

        string accessToken = TokenHandler.CreateToken(tokenDescriptor);

        return new TokenDto(accessToken, refreshToken);
    }

    public async Task<TokenDto> RefreshToken(TokenDto tokenDto)
    {
        ClaimsIdentity identity = await GetIdentityFromExpiredToken(tokenDto.AccessToken);

        if (string.IsNullOrEmpty(identity.Name))
            throw new InvalidOperationException("Name in expired token is not set.");

        User? user = await _userManager.FindByNameAsync(identity.Name);

        if (user?.RefreshToken == null || user.RefreshToken != tokenDto.RefreshToken ||
            user?.RefreshTokenExpiryTime <= DateTime.Now)
            throw new RefreshTokenBadRequest();

        _user = user;

        return await CreateToken(populateExp: false);
    }

    private static SigningCredentials GetSigningCredentials()
    {
        string? secretKey = Environment.GetEnvironmentVariable("SECRET");

        if (string.IsNullOrEmpty(secretKey))
            throw new InvalidOperationException("Environment variable \"SECRET\" is not set");

        byte[] key = Encoding.UTF8.GetBytes(secretKey);
        var secret = new SymmetricSecurityKey(key);

        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }

    private async Task<Dictionary<string, object>> GetClaims()
    {
        if (_user == null)
            throw new InvalidOperationException("User is not set in authentication service.");

        var claims = new Dictionary<string, object>
        {
            { ClaimTypes.Name, _user.UserName! },
        };

        IList<string> roles = await _userManager.GetRolesAsync(_user);
        claims.Add("roles", roles);
        return claims;
    }

    private SecurityTokenDescriptor GenerateTokenDescriptor(SigningCredentials signingCredentials,
        IDictionary<string, object> claims)
    {
        return new SecurityTokenDescriptor
        {
            Issuer = _jwtConfiguration.Issuer,
            Audience = _jwtConfiguration.Audience,
            Claims = claims,
            Expires = DateTime.Now.AddMinutes(Convert.ToDouble(_jwtConfiguration.Expires)),
            SigningCredentials = signingCredentials,
        };
    }

    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    private async Task<ClaimsIdentity> GetIdentityFromExpiredToken(string token)
    {
        string? secret = Environment.GetEnvironmentVariable("SECRET");

        if (string.IsNullOrEmpty(secret))
            throw new InvalidOperationException("Environment variable \"SECRET\" is not set");

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SECRET")!)),
            ValidateLifetime = false,
            ValidIssuer = _jwtConfiguration.Issuer,
            ValidAudience = _jwtConfiguration.Audience,
        };

        TokenValidationResult result = await TokenHandler.ValidateTokenAsync(
            token, tokenValidationParameters);

        if (!result.IsValid)
            throw result.Exception;

        return result.ClaimsIdentity;
    }
}
