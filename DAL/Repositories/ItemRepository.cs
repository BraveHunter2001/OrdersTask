using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public interface IItemRepository : IRepository<Item>
{
    Item? GetItemByCode(string code, bool readOnly = true);

    Dictionary<Guid, decimal> GetItemPriceDictionary(Guid[] ids);
}

internal class ItemRepository(OrdersTaskContext context) : Repository<Item>(context), IItemRepository
{
    public Item? GetItemByCode(string code, bool readOnly = true) =>
        (readOnly ? context.Items.AsNoTracking() : context.Items).FirstOrDefault(x => x.Code == code);

    public Dictionary<Guid, decimal> GetItemPriceDictionary(Guid[] ids)
    {
        return context.Items.Where(i => ids.Contains(i.Id))
            .ToDictionary(k => k.Id, v => v.Price);
    }
}