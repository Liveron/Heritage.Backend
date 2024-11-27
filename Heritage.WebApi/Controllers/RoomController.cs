using Heritage.Application.DataTransferObjects;
using Heritage.Application.RequestFeatures;
using Heritage.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Heritage.WebApi.Controllers;

[Route("api/rooms")]
[ApiController]
public class RoomController(IRoomsService service) : ControllerBase
{
    private readonly IRoomsService _service = service;

    [HttpPost(Name = "CreateRoom")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> CreateRoom([FromBody] CreateRoomDto room)
    {
        int id = await _service.CreateRoom(room);
        return CreatedAtRoute("GetRoom", new { id }, room);
    }

    [HttpGet(Name = "GetAllRooms")]
    public async Task<IActionResult> GetRooms([FromQuery] RoomParameters parameters)
    {
        var (rooms, metaData) = await _service.GetAllRooms(parameters);

        Response.Headers["X-Pagination"] = JsonSerializer.Serialize(metaData);

        return Ok(rooms); 
    }

    [HttpGet("{id:int}", Name = "GetRoom")]
    public async Task<IActionResult> GetRoom(int id)
    {
        RoomDto room = await _service.GetRoom(id);

        return Ok(room);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> UpdateRoom(int id, [FromBody] UpdateRoomDto updateRoomDto)
    {
        await _service.UpdateRoom(id, updateRoomDto);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> DeleteRoom(int id)
    {
        await _service.DeleteRoom(id);

        return NoContent();
    }
}
