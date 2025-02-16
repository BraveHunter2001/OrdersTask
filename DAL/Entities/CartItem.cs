namespace DAL.Entities;

public class CartItem
{
    public Guid Id { get; set; }

    public Guid CardId { get; set; }
    public Cart Cart { get; set; }

    public Guid ItemId { get; set; }
    public Item Item { get; set; }
    public int ItemsCount { get; set; }
}