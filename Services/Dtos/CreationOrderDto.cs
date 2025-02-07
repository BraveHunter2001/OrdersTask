namespace Services.Dtos;

public class CreationOrderDto
{
    public required OrderItemDto[] OrderItems { get; set; }
}

public class OrderItemDto
{
    public Guid ItemId { get; set; }
    public int Count { get; set; }
}