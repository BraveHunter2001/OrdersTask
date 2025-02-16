using DAL;
using DAL.Dto;
using DAL.Entities;
using DAL.Sequenser;
using Services.Dtos;

namespace Services;

public interface IOrderService
{
    Order CreateOrder(Customer customer, CreatingOrderDto creationOrderDto);
    bool TryDeleteOrder(Order order);
    void AcceptOrder(Order order);
    void CloseOrder(Order order);
    Order? GetOrderById(Guid id);
    Order? GetSimpleOrderById(Guid id);

    PaginatedContainer<List<Order>> GetPaginatedOrderList(OrderListFilter filter);
}

internal class OrderService(ICartService cartService, IUnitOfWork uow) : IOrderService
{
    public Order CreateOrder(Customer customer, CreatingOrderDto creationOrderDto)
    {
        Order order = new()
        {
            CustomerId = customer.Id,
            Status = OrderStatus.New,
            OrderDate = DateTime.Now,
        };

        Guid[] itemIds = creationOrderDto.Items.Select(x => x.ItemId).ToArray();
        var itemPriceDictionary = uow.ItemRepository.GetItemPriceDictionary(itemIds);

        OrderItem[] orderItems = creationOrderDto.Items.Select(x => new OrderItem
        {
            ItemsCount = x.Count,
            ItemId = x.ItemId,
            ItemPrice = itemPriceDictionary[x.ItemId]
        }).ToArray();

        order.OrderItems = orderItems;
        order.OrderNumber = uow.GetNextValueSequence<int>(SequenceType.OrderSequence);

        uow.OrderRepository.Insert(order);
        uow.Save();

        cartService.ChangeRangeItem(new ChangingCartItemDto()
        {
            CartItems = creationOrderDto.Items,
            Cart = customer.Cart,
            ActionType = CardActionType.RemoveItems
        });

        return order;
    }

    public bool TryDeleteOrder(Order order)
    {
        if (order.Status >= OrderStatus.InProgress)
            return false;

        uow.OrderRepository.Delete(order);
        uow.Save();

        return true;
    }

    public void AcceptOrder(Order order)
    {
        order.Status = OrderStatus.InProgress;
        order.ShipmentDate = DateTime.Now;

        uow.OrderRepository.Update(order);
        uow.Save();
    }

    public void CloseOrder(Order order)
    {
        order.Status = OrderStatus.Completed;

        uow.OrderRepository.Update(order);
        uow.Save();
    }

    public Order? GetOrderById(Guid id) => uow.OrderRepository.GetFullOrderById(id);
    public Order? GetSimpleOrderById(Guid id) => uow.OrderRepository.GetById(id);

    public PaginatedContainer<List<Order>> GetPaginatedOrderList(OrderListFilter filter) =>
        uow.OrderRepository.GetPaginatedOrderList(filter);
}