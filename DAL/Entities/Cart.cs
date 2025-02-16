namespace DAL.Entities;

public class Cart
{
    public Guid Id { get; set; }

    public List<CartItem> Items { get; set; }

    public Guid CustomerId { get; set; }
    public Customer Customer { get; set; }
}