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
    public class ShopBuyVehicle : IShop
    {
        private readonly ShopsPlugin m_ShopsPlugin;
        private readonly IStringLocalizer m_StringLocalizer;
        private readonly IEconomyProvider m_EconomyProvider;
        private readonly IEventBus m_EventBus;

        public ShopBuyVehicle(BuyVehicle shop,
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

        public async UniTask Interact(UnturnedUser user, int amount = 1)
        {
            if (amount != 1)
            {
                throw new ArgumentException("Parameter must equal one", "amount");
            }

            decimal newBalance;

            VehicleAsset asset = (VehicleAsset)Assets.find(EAssetType.VEHICLE, Id);

            if (asset == null)
            {
                throw new Exception($"Vehicle asset for Id '{Id}' not found");
            }

            var buyingEvent = new PlayerBuyingVehicleEvent(user, Id, Price);
            await m_EventBus.EmitAsync(m_ShopsPlugin, this, buyingEvent);

            if (buyingEvent.IsCancelled) return;

            newBalance = await m_EconomyProvider.UpdateBalanceAsync(user.Id, user.Type, -Price, "Purchase of vehicle " + asset.vehicleName);

            await UniTask.SwitchToMainThread();

            VehicleTool.giveVehicle(user.Player.Player, Id);

            await user.PrintMessageAsync(m_StringLocalizer["shops:success:item_buy",
                new
                {
                    VehicleName = asset.vehicleName,
                    VehicleId = asset.id,
                    BuyPrice = Price,
                    Balance = newBalance,
                    m_EconomyProvider.CurrencyName,
                    m_EconomyProvider.CurrencySymbol,
                }]);

            var boughtEvent = new PlayerBoughtVehicleEvent(user, Id, Price);
            await m_EventBus.EmitAsync(m_ShopsPlugin, this, boughtEvent);
        }
    }
}
