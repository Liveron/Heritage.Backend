using Heritage.Application.Common.Exceptions;
using Heritage.Application.Interfaces;
using Heritage.Application.RequestFeatures;
using Heritage.Domain;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Heritage.Persistance.Repositories;

public class OrderRepository(HeritageDbContext context) : IOrderRepository
{
    private readonly HeritageDbContext _context = context;

    public async Task<Guid> CreateOrderAsync(Order order)
    {
        await _context.Orders.AddAsync(order);
        return order.Id;
    }

    public async Task DeleteOrderAsync(Guid id)
    {
        Order order = await _context.Orders.AsNoTracking()
            .FirstOrDefaultAsync(order => order.Id == id) ??
            throw new OrderNotFoundException(id);

        _context.Orders.Remove(order);
    }

    public async Task<PagedList<Order>> GetAllOrdersAsync(OrderParameters parameters)
    {
        List<Order> orders = await _context.Orders.AsNoTracking()
            .Include(order => order.User)
            .Skip(parameters.PageSize * (parameters.PageNumber - 1))
            .Take(parameters.PageSize)
            .ToListAsync();

        int count = await _context.Orders.CountAsync();

        return new PagedList<Order>(orders, count, parameters.PageNumber, parameters.PageSize);
    }

    public async Task<Order> GetOrderAsync(Guid id)
    {
        Order order = await _context.Orders.AsNoTracking()
            .FirstOrDefaultAsync(order => order.Id == id) ??
            throw new OrderNotFoundException(id);

        return order;
    }

    public async Task UpdateOrderAsync(Order order)
    {
        _ = await _context.Orders.AsNoTracking()
            .FirstOrDefaultAsync(order => order.Id == order.Id) ?? 
            throw new OrderNotFoundException(order.Id);

        _context.Entry(order).State = EntityState.Modified;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
