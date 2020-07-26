using Cysharp.Threading.Tasks;
using Microsoft.Extensions.Localization;
using OpenMod.Extensions.Economy.Abstractions;
using OpenMod.Unturned.Users;
using SDG.Unturned;
using Shops.Database.Models;
using System;

namespace Shops.Shops
{
    public class ShopBuyVehicle : IShop
    {
        private readonly IStringLocalizer m_StringLocalizer;
        private readonly IEconomyProvider m_EconomyProvider;

        public ShopBuyVehicle(BuyVehicle shop,
            IStringLocalizer stringLocalizer,
            IEconomyProvider economyProvider)
        {
            ID = (ushort)shop.ID;
            Price = shop.BuyPrice;

            m_EconomyProvider = economyProvider;
        }

        public ushort ID;

        public decimal Price;

        public async UniTask Interact(UnturnedUser user, int amount = 1)
        {
            if (amount != 1)
            {
                throw new ArgumentException("Parameter must equal one", "amount");
            }

            decimal newBalance;

            VehicleAsset asset = (VehicleAsset)Assets.find(EAssetType.VEHICLE, ID);

            if (asset == null)
            {
                throw new Exception($"Vehicle asset for ID '{ID}' not found");
            }

            newBalance = await m_EconomyProvider.UpdateBalanceAsync(user.Id, user.Type, -Price);

            await UniTask.SwitchToMainThread();

            VehicleTool.giveVehicle(user.Player, ID);

            await user.PrintMessageAsync(m_StringLocalizer["shops:success:item_buy",
                new
                {
                    VehicleName = asset.vehicleName,
                    VehicleID = asset.id,
                    BuyPrice = Price,
                    Balance = newBalance,
                    m_EconomyProvider.CurrencyName,
                    m_EconomyProvider.CurrencySymbol,
                }]);
        }
    }
}
