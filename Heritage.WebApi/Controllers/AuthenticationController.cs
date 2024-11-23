using Heritage.Application.DataTransferObjects;
using Heritage.Application.Services;
using Heritage.Domain;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Heritage.WebApi.Controllers;

[Route("api/authentication")]
[ApiController]
public class AuthenticationController(IAuthenticationService service) : ControllerBase
{
    private readonly IAuthenticationService _service = service;

    [HttpPost]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDto userDto)
    {
        IdentityResult result = await _service.RegisterUser(userDto);

        if (!result.Succeeded)
        {
            foreach (IdentityError error in result.Errors)
            {
                ModelState.TryAddModelError(error.Code, error.Description);
            }

            return BadRequest(ModelState);
        }

        return Created();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Authenticate([FromBody] AuthenticateUserDto userDto)
    {
        if (!await _service.ValidateUser(userDto)) 
            return Unauthorized();

        TokenDto tokenDto = await _service.CreateToken(populateExp: true);

        return Ok(tokenDto);
    }
}
