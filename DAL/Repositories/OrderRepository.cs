using DAL.Dto;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public interface IOrderRepository : IRepository<Order>
{
    Order? GetFullOrderById(Guid id);

    PaginatedContainer<List<Order>> GetPaginatedOrderList(OrderListFilter filter);
}

internal class OrderRepository(OrdersTaskContext context) : Repository<Order>(context), IOrderRepository
{
    public Order? GetFullOrderById(Guid id)
    {
        return context.Orders
            .Include(o => o.OrderItems).ThenInclude(oi => oi.Item)
            .FirstOrDefault(o => o.Id == id);
    }

    public PaginatedContainer<List<Order>> GetPaginatedOrderList(OrderListFilter filter)
    {
        var query = context.Orders
            .AsNoTracking()
            .Where(o => !filter.Status.HasValue || o.Status == filter.Status)
            .OrderByDescending(o => o.OrderDate);

        var result = GetPaginatedListContainer(query, filter);
        return result;
    }
}