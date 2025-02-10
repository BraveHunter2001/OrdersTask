using DAL.Entities;

namespace Services.Dtos;

public class ItemDto
{
    public string? Name { get; set; }
    public decimal Price { get; set; }
    public string? Category { get; set; }

    public Item ToItem() => new(Name, Price, Category);
}