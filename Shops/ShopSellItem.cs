using Cysharp.Threading.Tasks;
using Microsoft.Extensions.Localization;
using OpenMod.API.Commands;
using OpenMod.Extensions.Economy.Abstractions;
using OpenMod.Unturned.Users;
using SDG.Unturned;
using Shops.Database.Models;
using System;
using System.Collections.Generic;

namespace Shops.Shops
{
    public class ShopSellItem : IShop
    {
        private readonly IStringLocalizer m_StringLocalizer;
        private readonly IEconomyProvider m_EconomyProvider;

        public ShopSellItem(SellItem shop,
            IStringLocalizer stringLocalizer,
            IEconomyProvider economyProvider)
        {
            ID = (ushort)shop.ID;
            Price = shop.SellPrice;

            m_StringLocalizer = stringLocalizer;
            m_EconomyProvider = economyProvider;
        }

        public ushort ID;

        public decimal Price;

        public async UniTask Interact(UnturnedUser user, int amount)
        {
            await UniTask.SwitchToMainThread();

            decimal totalPrice = Price * amount;

            ItemAsset asset = (ItemAsset)Assets.find(EAssetType.ITEM, ID);

            if (asset == null)
            {
                throw new Exception($"Item asset for ID '{ID}' not found");
            }

            await UniTask.SwitchToMainThread();

            List<InventorySearch> foundItems = user.Player.Player.inventory.search(ID, true, true);

            if (foundItems.Count < amount)
            {
                throw new UserFriendlyException(m_StringLocalizer["item_sell_not_enough", new
                {
                    ItemName = asset.itemName,
                    ItemID = asset.id,
                    Amount = amount
                }]);
            }

            for (int i = 0; i < amount; i++)
            {
                InventorySearch found = foundItems[i];

                byte index = user.Player.Player.inventory.getIndex(found.page, found.jar.x, found.jar.y);

                user.Player.Player.inventory.removeItem(found.page, index);
            }

            await UniTask.SwitchToThreadPool();

            decimal newBalance = await m_EconomyProvider.UpdateBalanceAsync(user.Id, user.Type, totalPrice);

            await user.PrintMessageAsync(m_StringLocalizer["shops:success:item_sell",
                new
                {
                    ItemName = asset.itemName,
                    ItemID = asset.id,
                    Amount = amount,
                    SellPrice = totalPrice,
                    Balance = newBalance,
                    m_EconomyProvider.CurrencyName,
                    m_EconomyProvider.CurrencySymbol,
                }]);
        }
    }
}
