using DAL.Entities;

namespace WebApi.ViewModel
{
    public class CartItemViewModel(CartItem cardItem) : ItemListViewModel(cardItem.Item)
    {
        public Guid CartItemId { get; set; } = cardItem.Id;
        public int Count { get; set; } = cardItem.ItemsCount;
    }
}