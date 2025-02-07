using DAL;
using DAL.Entities;
using DAL.Sequenser;
using Services.Dtos;

namespace Services;

public interface IOrderService
{
    Order CreateOrder(Customer customer, CreationOrderDto creationOrderDto);
    bool TryDeleteOrder(Order order);
}

internal class OrderServices(IUnitOfWork uow) : IOrderService
{
    public Order CreateOrder(Customer customer, CreationOrderDto creationOrderDto)
    {
        Order order = new()
        {
            CustomerId = customer.Id,
            Status = OrderStatus.New,
            OrderDate = DateTime.Now,
            OrderNumber = uow.GetNextValueSequence<int>(SequenceType.OrderSequence)
        };

        uow.OrderRepository.Insert(order);

        Guid[] itemIds = creationOrderDto.OrderItems.Select(x => x.ItemId).ToArray();
        var itemPriceDictionary = uow.ItemRepository.GetItemPriceDictionary(itemIds);

        OrderItem[] orderItems = creationOrderDto.OrderItems.Select(x => new OrderItem
        {
            OrderId = order.Id,
            ItemsCount = x.Count,
            ItemId = x.ItemId,
            ItemPrice = itemPriceDictionary[x.ItemId]
        }).ToArray();

        order.OrderItems = orderItems;
        uow.Save();

        return order;
    }

    public bool TryDeleteOrder(Order order)
    {
        if (order.Status == OrderStatus.Completed)
            return false;

        uow.OrderRepository.Delete(order);
        uow.Save();

        return true;
    }
}