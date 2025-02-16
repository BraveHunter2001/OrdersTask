using DAL.Entities;

namespace Services.Dtos;

public class ChangingCartItemDto
{
    public required BaseItemDto[] CartItems { get; set; }
    public Cart? Cart { get; set; }

    public CardActionType ActionType { get; set; }
}

public enum CardActionType
{
    AddItems,
    RemoveItems
}

public class BaseItemDto
{
    public Guid ItemId { get; set; }
    public int Count { get; set; }
}