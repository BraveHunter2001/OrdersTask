using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public interface IOrderRepository : IRepository<Order>
{
    public Order? GetFullOrderById(Guid id);
}

internal class OrderRepository(OrdersTaskContext context) : Repository<Order>(context), IOrderRepository
{
    public Order? GetFullOrderById(Guid id)
    {
        return context.Orders
            .Include(o => o.OrderItems).ThenInclude(oi => oi.Item)
            .FirstOrDefault(o => o.Id == id);
    }
}