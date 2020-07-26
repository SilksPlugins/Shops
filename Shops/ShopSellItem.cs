using Cysharp.Threading.Tasks;
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
        private readonly IEconomyProvider m_EconomyProvider;

        public ShopSellItem(SellItem shop, IEconomyProvider economyProvider)
        {
            ID = (ushort)shop.ID;
            Price = shop.SellPrice;

            m_EconomyProvider = economyProvider;
        }

        public ushort ID;

        public decimal Price;

        public async UniTask Interact(UnturnedUser user, int amount)
        {
            await UniTask.SwitchToMainThread();

            decimal totalPrice = Price * amount;

            List<InventorySearch> foundItems = user.Player.inventory.search(ID, true, true);

            if (foundItems.Count < amount)
            {
                await user.PrintMessageAsync("You do not have enough of this item.");
                return;
            }

            ItemAsset asset = (ItemAsset)Assets.find(EAssetType.ITEM, ID);

            if (asset == null)
            {
                throw new Exception($"Item asset for ID '{ID}' not found");
            }

            await UniTask.SwitchToMainThread();

            foreach (InventorySearch found in foundItems)
            {
                byte index = user.Player.inventory.getIndex(found.page, found.jar.x, found.jar.y);

                user.Player.inventory.removeItem(found.page, index);
            }

            await UniTask.SwitchToThreadPool();

            decimal newBalance = await m_EconomyProvider.UpdateBalanceAsync(user.Id, user.Type, totalPrice);

            await user.PrintMessageAsync($"Successfully sold {amount} {asset.itemName} for ${totalPrice}. Your balance is now ${newBalance}.");
        }
    }
}
