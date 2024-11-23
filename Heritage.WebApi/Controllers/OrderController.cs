using Heritage.Application.DataTransferObjects;
using Heritage.Application.RequestFeatures;
using Heritage.Application.Services;
using Heritage.WebApi.DataTransferObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Heritage.WebApi.Controllers;


[Route("api/orders")]
[ApiController]
public class OrderController(IOrderService service) : ControllerBase
{
    private readonly IOrderService _service = service;

    [HttpPost(Name = "CreateOrder")]
    [Authorize]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto orderDto)
    {
        Guid id = await _service.CreateOrder(orderDto, User.Identity!.Name!);

        return CreatedAtRoute("GetOrder", new { id, }, orderDto);
    }

    [HttpGet("{id:guid}", Name = "GetOrder")]
    [Authorize]
    public async Task<IActionResult> GetOrder(Guid id)
    {
        OrderDto order = await _service.GetOrder(id, User.Identity.Name);

        return Ok(order);
    }

    [HttpGet(Name = "GetAllOrders")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> GetOrders([FromQuery] OrderParameters parameters)
    {
        var (orders, metaData) = await _service.GetAllOrders(parameters);

        Response.Headers["X-Pagination"] = JsonSerializer.Serialize(metaData);

        return Ok(orders);
    }

    [HttpPut("{id:guid}", Name = "UpdateOrder")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> UpdateOrder(Guid id, [FromBody] UpdateOrderDto orderDto)
    {
        if (orderDto is null)
            return BadRequest("Order object is null");

        await _service.UpdateOrder(id, orderDto);

        return NoContent();
    }

    [HttpDelete("{id:guid}", Name = "DeleteOrder")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> DeleteOrder(Guid id)
    {
        await _service.DeleteOrder(id);

        return NoContent(); 
    }
}
