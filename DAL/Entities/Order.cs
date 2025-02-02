namespace DAL.Entities;

public class Order
{
    public Guid Id { get; set; }

    /// <summary>
    /// Дата создания
    /// </summary>
    public DateTime OrderDate { get; set; }

    public DateTime ShipmentDate { get; set; }
    public int OrderNumber { get; set; }
    public OrderStatus Status { get; set; }

    public Guid CustomerId { get; set; }
    public required Customer Customer { get; set; }

    public ICollection<OrderItem> OrderItems { get; set; }
}

public enum OrderStatus
{
    New,
    InProgress,
    Completed
}