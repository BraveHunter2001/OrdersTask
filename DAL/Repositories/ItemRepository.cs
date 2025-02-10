using DAL.Dto;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public interface IItemRepository : IRepository<Item>
{
    Item? GetItemByCode(string code, bool readOnly = true);

    Dictionary<Guid, decimal> GetItemPriceDictionary(Guid[] ids);
    PaginatedContainer<List<Item>> GetPaginatedItemList(ItemListFilter filter);

    SuggestDto<string>[] GetCategorySuggest(string query);
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

    public PaginatedContainer<List<Item>> GetPaginatedItemList(ItemListFilter filter)
    {
        bool isEmptyCategories = filter.Categories == null || filter.Categories.Length == 0;
        var query = context.Items
            .AsNoTracking()
            .Where(i =>
                (string.IsNullOrWhiteSpace(filter.Code) || i.Code.StartsWith(filter.Code))
                && (isEmptyCategories || filter.Categories.Contains(i.Category))
            ).OrderByDescending(i => i.Code);

        var result = GetPaginatedListContainer(query, filter);
        return result;
    }

    public SuggestDto<string>[] GetCategorySuggest(string query)
    {
        string lowerQuery = query.ToLower();
        var suggests = context.Items
            .AsNoTracking()
            .Where(i => i.Category.ToLower().Contains(lowerQuery))
            .GroupBy(i => i.Category)
            .Select(g => new SuggestDto<string>() { Label = g.Key, Value = g.Key })
            .ToArray();

        return suggests;
    }
}