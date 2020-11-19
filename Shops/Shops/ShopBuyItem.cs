using Cysharp.Threading.Tasks;
using Microsoft.Extensions.Localization;
using OpenMod.API.Eventing;
using OpenMod.Extensions.Economy.Abstractions;
using OpenMod.Unturned.Users;
using SDG.Unturned;
using Shops.Database.Models;
using Shops.Events;
using System;

namespace Shops.Shops
{
    public class ShopBuyItem : IShop
    {
        private readonly ShopsPlugin m_ShopsPlugin;
        private readonly IStringLocalizer m_StringLocalizer;
        private readonly IEconomyProvider m_EconomyProvider;
        private readonly IEventBus m_EventBus;

        public ShopBuyItem(BuyItem shop,
            ShopsPlugin shopsPlugin,
            IStringLocalizer stringLocalizer,
            IEconomyProvider economyProvider,
            IEventBus eventBus)
        {
            Id = (ushort)shop.Id;
            Price = shop.BuyPrice;

            m_ShopsPlugin = shopsPlugin;
            m_StringLocalizer = stringLocalizer;
            m_EconomyProvider = economyProvider;
            m_EventBus = eventBus;
        }

        public ushort Id;

        public decimal Price;

        public async UniTask Interact(UnturnedUser user, int amount)
        {
            decimal totalPrice = Price * amount;

            decimal newBalance;

            ItemAsset asset = (ItemAsset)Assets.find(EAssetType.ITEM, Id);

            if (asset == null)
            {
                throw new Exception($"Item asset for Id '{Id}' not found");
            }

            var buyingEvent = new PlayerBuyingItemEvent(user, Id, amount, Price);
            await m_EventBus.EmitAsync(m_ShopsPlugin, this, buyingEvent);

            if (buyingEvent.IsCancelled) return;

            newBalance = await m_EconomyProvider.UpdateBalanceAsync(user.Id, user.Type, -totalPrice, $"Purchase of {amount} {asset.itemName}s");

            await UniTask.SwitchToMainThread();

            for (int i = 0; i < amount; i++)
            {
                Item item = new Item(Id, EItemOrigin.ADMIN);

                user.Player.Player.inventory.forceAddItem(item, true);
            }

            await user.PrintMessageAsync(m_StringLocalizer["shops:success:item_buy",
                new
                {
                    ItemName = asset.itemName,
                    ItemId = asset.id,
                    Amount = amount,
                    BuyPrice = totalPrice,
                    Balance = newBalance,
                    m_EconomyProvider.CurrencyName,
                    m_EconomyProvider.CurrencySymbol,
                }]);

            var boughtEvent = new PlayerBoughtItemEvent(user, Id, amount, Price);
            await m_EventBus.EmitAsync(m_ShopsPlugin, this, boughtEvent);
        }
    }
}
