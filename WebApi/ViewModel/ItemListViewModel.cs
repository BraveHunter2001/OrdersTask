using DAL.Entities;

namespace WebApi.ViewModel;

public class ItemListViewModel(Item item)
{
    public Guid ItemId { get; set; } = item.Id;
    public string Name { get; set; } = item.Name;
    public string Code { get; set; } = item.Code;
    public decimal Price { get; set; } = item.Price;
    public string Category { get; set; } = item.Category;
}