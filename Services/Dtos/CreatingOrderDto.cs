using DAL.Entities;

namespace Services.Dtos;

public class CreatingOrderDto
{
    public required BaseItemDto[] Items { get; set; }

    public Customer? Customer { get; set; }
}
