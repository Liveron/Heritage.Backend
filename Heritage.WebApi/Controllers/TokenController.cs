using Heritage.Application.DataTransferObjects;
using Heritage.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Heritage.WebApi.Controllers;

[Route("api/token")]
[ApiController]
public class TokenController(IAuthenticationService service) : ControllerBase
{
    private readonly IAuthenticationService _service = service;

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] TokenDto tokenDto)
    {
        TokenDto tokenDtoToReturn = await _service.RefreshToken(tokenDto);

        return Ok(tokenDtoToReturn);
    }
}
