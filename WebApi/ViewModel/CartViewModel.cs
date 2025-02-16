using DAL.Entities;

namespace WebApi.ViewModel;

public class CartViewModel
{
    public List<CartItemViewModel> Items { get; set; }

    public CartViewModel(Cart cart)
    {
        Items = cart.Items.ConvertAll(i => new CartItemViewModel(i));
    }
}