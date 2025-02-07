using DAL.Entities;

namespace Services.Dtos;

public class ItemDto
{
    public required string Name { get; set; }
    public decimal Price { get; set; }
    public required string Category { get; set; }

    public Item ToItem() => new(Name, Price, Category);
}