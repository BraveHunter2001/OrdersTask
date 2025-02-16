using DAL.Entities;

namespace WebApi.ViewModel;

public class OrderListItemViewModel(Order order)
{
    public Guid OrderId { get; set; } = order.Id; 
    public int Number { get; set; } = order.OrderNumber;
    public OrderStatus Status { get; set; } = order.Status;
    public string StatusName => Status.ToString();

    public string CreateDate { get; set; } = order.OrderDate.ToShortDateString();
    public string? ShippingDate { get; set; } = order.ShipmentDate?.ToShortDateString();
    public decimal? OrderPrice { get; set; } = order.OrderItems?.Sum(oi=>oi.ItemPrice * oi.ItemsCount);
}