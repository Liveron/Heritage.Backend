using Heritage.Application.Common.Exceptions;
using Heritage.Application.DataTransferObjects;
using Heritage.Application.Interfaces;
using Heritage.Application.RequestFeatures;
using Heritage.Domain;
using Mapster;
using Microsoft.AspNetCore.Identity;

namespace Heritage.Application.Services;

public interface IOrderService
{
    public Task<Guid> CreateOrder(CreateOrderDto orderDto, string userName);
    public Task<OrderDto> GetOrder(Guid id, string userId);
    public Task<(List<OrderDto> orders, MetaData metaData)> GetAllOrders(OrderParameters parameters);
    public Task DeleteOrder(Guid id);
    public Task UpdateOrder(Guid id, UpdateOrderDto orderDto);
}

public class OrderService(IOrderRepository repository, UserManager<User> userManager) : IOrderService
{
    private readonly IOrderRepository _repository = repository;
    private readonly UserManager<User> _userManager = userManager;

    public async Task<Guid> CreateOrder(CreateOrderDto orderDto, string userName)
    {
        User user = await _userManager.FindByNameAsync(userName) ?? 
            throw new UserNotFoundByUsernameException(userName);

        Order order = orderDto.BuildAdapter()
            .AddParameters("id", user.Id)
            .AdaptToType<Order>();

        Guid id = await _repository.CreateOrderAsync(order);
        await _repository.SaveChangesAsync();
        return id; 
    }

    public async Task<(List<OrderDto> orders, MetaData metaData)> GetAllOrders(OrderParameters parameters)
    {
        PagedList<Order> ordersWithMetaData = await _repository.GetAllOrdersAsync(parameters);

        var orders = new List<OrderDto>(ordersWithMetaData.Count);

        foreach (Order order in ordersWithMetaData)
        {
            OrderDto orderDto = order.BuildAdapter()
                .AddParameters("username", order.User!.UserName!)
                .AdaptToType<OrderDto>();

            orders.Add(orderDto);
        }

        return (orders, metaData: ordersWithMetaData.MetaData);
    }

    public async Task<OrderDto> GetOrder(Guid id, string userName)
    {
        Order orderDb =  await _repository.GetOrderAsync(id);

        User user = await _userManager.FindByIdAsync(orderDb.UserId) ?? 
            throw new UserNotFoundByIdException(id);

        if (user.UserName != userName || !await _userManager.IsInRoleAsync(user, "Administrator"))
            throw new OrderNotAllowedException(user.UserName);

        OrderDto order = orderDb.BuildAdapter()
            .AddParameters("username", user.UserName)
            .AdaptToType<OrderDto>();

        return order;
    }

    public async Task DeleteOrder(Guid id)
    {
        await _repository.DeleteOrderAsync(id);
        await _repository.SaveChangesAsync();
    }

    public async Task UpdateOrder(Guid id, UpdateOrderDto orderDto)
    {
        User user = await _userManager.FindByNameAsync(orderDto.UserName) ??
            throw new UserNotFoundByUsernameException(orderDto.UserName);

        Order order = orderDto.BuildAdapter()
            .AddParameters("id", id)
            .AddParameters("userId", user.Id)
            .AdaptToType<Order>();

        await _repository.UpdateOrderAsync(order);
        await _repository.SaveChangesAsync();
    }
}
