using Heritage.Application.Models;
using Heritage.Application.RequestFeatures;
using Heritage.Domain;

namespace Heritage.Application.Interfaces;

public interface IOrderRepository
{
    public Task<Guid> CreateOrderAsync(Order order);
    public Task UpdateOrderAsync(Order article);
    public Task DeleteOrderAsync(Guid id);
    public Task<PagedList<Order>> GetAllOrdersAsync(OrderParameters parameters);
    public Task<Order> GetOrderAsync(Guid id);
    public Task SaveChangesAsync();
}
