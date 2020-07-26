using Cysharp.Threading.Tasks;
using Microsoft.Extensions.Localization;
using OpenMod.Extensions.Economy.Abstractions;
using OpenMod.Unturned.Users;
using SDG.Unturned;
using Shops.Database.Models;
using System;

namespace Shops.Shops
{
    public class ShopBuyItem : IShop
    {
        private readonly IStringLocalizer m_StringLocalizer;
        private readonly IEconomyProvider m_EconomyProvider;

        public ShopBuyItem(BuyItem shop,
            IStringLocalizer stringLocalizer,
            IEconomyProvider economyProvider)
        {
            ID = (ushort)shop.ID;
            Price = shop.BuyPrice;

            m_StringLocalizer = stringLocalizer;
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

            await user.PrintMessageAsync(m_StringLocalizer["shops:success:item_buy",
                new
                {
                    ItemName = asset.itemName,
                    ItemID = asset.id,
                    Amount = amount,
                    BuyPrice = totalPrice,
                    Balance = newBalance,
                    m_EconomyProvider.CurrencyName,
                    m_EconomyProvider.CurrencySymbol,
                }]);
        }
    }
}
