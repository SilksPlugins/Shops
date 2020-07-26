using Cysharp.Threading.Tasks;
using OpenMod.Extensions.Economy.Abstractions;
using OpenMod.Unturned.Users;
using SDG.Unturned;
using Shops.Database.Models;
using System;

namespace Shops.Shops
{
    public class ShopBuyItem : IShop
    {
        private readonly IEconomyProvider m_EconomyProvider;

        public ShopBuyItem(BuyItem shop, IEconomyProvider economyProvider)
        {
            ID = (ushort)shop.ID;
            Price = shop.BuyPrice;

            m_EconomyProvider = economyProvider;
        }

        public ushort ID;

        public decimal Price;

        public async UniTask Interact(UnturnedUser user, int amount)
        {
            decimal totalPrice = Price * amount;

            decimal newBalance;

            ItemAsset asset = (ItemAsset)Assets.find(EAssetType.ITEM, ID);

            if (asset == null)
            {
                throw new Exception($"Item asset for ID '{ID}' not found");
            }

            newBalance = await m_EconomyProvider.UpdateBalanceAsync(user.Id, user.Type, -totalPrice);

            await UniTask.SwitchToMainThread();

            for (int i = 0; i < amount; i++)
            {
                Item item = new Item(ID, EItemOrigin.ADMIN);

                user.Player.inventory.forceAddItem(item, true);
            }

            await user.PrintMessageAsync($"Successfully purchased {amount} {asset.itemName} for ${totalPrice}. Your balance is now ${newBalance}.");
        }
    }
}
