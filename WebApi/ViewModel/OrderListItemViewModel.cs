using DAL.Entities;

namespace WebApi.ViewModel;

public class OrderListItemViewModel
{
    public Guid OrderId { get; set; }
    public int Number { get; set; }
    public OrderStatus Status { get; set; }
    public string StatusName { get; set; }

    public string CreateDate { get; set; }
    public string? ShippingDate { get; set; }
    public decimal? OrderPrice { get; set; }
    public decimal? OrderPriceWithDiscount { get; set; }

    public OrderListItemViewModel(Order order)
    {
        OrderId = order.Id;
        Number = order.OrderNumber;
        Status = order.Status;
        StatusName = Status.ToString();
        CreateDate = order.OrderDate.ToShortDateString();
        ShippingDate = order.ShipmentDate?.ToShortDateString();
        OrderPrice = order.OrderItems?.Sum(oi => oi.ItemPrice * oi.ItemsCount);
        OrderPriceWithDiscount = OrderPrice * ((100 - (order.Customer.Discount ?? 0)) / 100);
    }
}