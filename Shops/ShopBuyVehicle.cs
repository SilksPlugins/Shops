using Cysharp.Threading.Tasks;
using OpenMod.Extensions.Economy.Abstractions;
using OpenMod.Unturned.Users;
using SDG.Unturned;
using Shops.Database.Models;
using System;

namespace Shops.Shops
{
    public class ShopBuyVehicle : IShop
    {
        private readonly IEconomyProvider m_EconomyProvider;

        public ShopBuyVehicle(BuyVehicle shop, IEconomyProvider economyProvider)
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

            await user.PrintMessageAsync($"Successfully purchased {asset.vehicleName} for ${Price}. Your balance is now ${newBalance}.");
        }
    }
}
