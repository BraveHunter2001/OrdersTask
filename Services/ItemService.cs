using DAL;
using DAL.Entities;
using DAL.Sequenser;
using Services.Dtos;

namespace Services;

public interface IItemService
{
    Item CreateItem(ItemDto itemDto);
    void UpdateItem(Item origin, UpdatingItemDto updatingDto);
    void DeleteItem(Item item);
    Item? GetItemById(Guid id);
}

internal class ItemService(IUnitOfWork uow) : IItemService
{
    public Item CreateItem(ItemDto itemDto)
    {
        Item item = itemDto.ToItem();

        item.Code = GenerateCode();
        uow.ItemRepository.Insert(item);
        uow.Save();
        return item;
    }

    public void UpdateItem(Item origin, UpdatingItemDto updatingDto)
    {
        if (!string.IsNullOrWhiteSpace(updatingDto.Name))
            origin.Name = updatingDto.Name;

        if (!string.IsNullOrWhiteSpace(updatingDto.Code))
            origin.Code = updatingDto.Code;

        if (!string.IsNullOrWhiteSpace(updatingDto.Category))
            origin.Category = updatingDto.Category;

        if (updatingDto.Price.HasValue)
            origin.Price = updatingDto.Price.Value;

        uow.ItemRepository.Update(origin);
        uow.Save();
    }

    public void DeleteItem(Item item)
    {
        uow.ItemRepository.Delete(item);
        uow.Save();
    }

    public Item? GetItemById(Guid id) => uow.ItemRepository.GetById(id);

    private string GenerateCode()
    {
        const int startChar = (int) ('A');

        long counter = uow.GetNextValueSequence<long>(SequenceType.ItemSequence);

        int lettersNum = (int) (counter % 10000) / 100;

        char c1 = (char) (startChar + lettersNum % 26);
        char c2 = (char) (startChar + lettersNum / 26);
        string letters = new string([c2, c1]);

        string num = counter.ToString("d10");

        //01 2345 6789
        //XX-XXXX-YYXX
        return $"{num.Substring(0, 2)}-{num.Substring(2, 4)}-{letters}{num.Substring(8, 2)}";
    }
}