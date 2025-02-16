using DAL;
using DAL.Entities;
using Services.Dtos;

namespace Services;

public interface ICartService
{
    void ChangeRangeItem(ChangingCartItemDto request);

    bool TryChangeCardItemCount(Guid cartItemId, int count);
    bool TryDeleteCardItem(Guid cartItemId);
}

internal class CartService(IUnitOfWork uow) : ICartService
{
    public void ChangeRangeItem(ChangingCartItemDto request)
    {
        Cart cart = request.Cart;

        switch (request.ActionType)
        {
            case CardActionType.AddItems:
                var cardItems = request.CartItems.Select(x => new CartItem
                {
                    ItemsCount = x.Count,
                    ItemId = x.ItemId,
                }).ToList();

                var cardItemSet = cart.Items.Select(x => x.ItemId).ToHashSet();

                var newItems = cardItems
                    .Where(i => !cardItemSet.Contains(i.ItemId))
                    .ToList();

                var exitedItems = cardItems
                    .Where(i => cardItemSet.Contains(i.ItemId))
                    .ToDictionary(k => k.ItemId);

                if (exitedItems.Count > 0)
                    foreach (var item in cart.Items)
                    {
                        if (exitedItems.TryGetValue(item.ItemId, out CartItem? exitedItem))
                            item.ItemsCount += exitedItem.ItemsCount;
                    }

                if (newItems.Count > 0)
                    cart.Items.AddRange(newItems);

                break;

            case CardActionType.RemoveItems:

                var cardItemsDict = request.CartItems.Select(x => new CartItem
                {
                    ItemsCount = x.Count,
                    ItemId = x.ItemId,
                }).ToDictionary(k => k.ItemId, v => v.ItemsCount);

                var excitedItems = cart.Items
                    .Where(i => cardItemsDict.ContainsKey(i.ItemId))
                    .ToList();

                var removedItems = excitedItems
                    .Where(i => i.ItemsCount == cardItemsDict[i.ItemId])
                    .Select(i => i.ItemId)
                    .ToHashSet();

                cart.Items.RemoveAll(i => removedItems.Contains(i.ItemId));

                var lowerCountItems = excitedItems
                    .Where(i => i.ItemsCount > cardItemsDict[i.ItemId])
                    .ToList();

                foreach (var item in lowerCountItems)
                {
                    if (cardItemsDict.TryGetValue(item.ItemId, out int count))
                        item.ItemsCount -= count;
                }

                break;
        }

        uow.CartRepository.Update(cart);
        uow.Save();
    }

    public bool TryChangeCardItemCount(Guid cartItemId, int count)
    {
        var cartItem = uow.CartItemRepository.GetById(cartItemId);

        if (cartItem is null) return false;

        cartItem.ItemsCount = count;

        uow.CartItemRepository.Update(cartItem);
        uow.Save();

        return true;
    }

    public bool TryDeleteCardItem(Guid cartItemId)
    {
        var cartItem = uow.CartItemRepository.GetById(cartItemId);

        if (cartItem is null) return false;

        uow.CartItemRepository.Delete(cartItem);
        uow.Save();

        return true;
    }
}