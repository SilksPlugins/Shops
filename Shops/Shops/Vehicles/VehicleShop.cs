using Microsoft.Extensions.Localization;
using OpenMod.Extensions.Economy.Abstractions;
using OpenMod.Extensions.Games.Abstractions.Players;
using OpenMod.Extensions.Games.Abstractions.Vehicles;
using Shops.API.Vehicles;
using Shops.Database.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Shops.Shops.Vehicles
{
    public class VehicleShop : IVehicleShop
    {
        private readonly IEconomyProvider _economyProvider;
        private readonly IVehicleDirectory _vehicleDirectory;
        private readonly IVehicleSpawner _vehicleSpawner;
        private readonly IStringLocalizer _stringLocalizer;

        public IVehicleShopData ShopData { get; }

        public VehicleShop(IEconomyProvider economyProvider,
            IVehicleDirectory vehicleDirectory,
            IVehicleSpawner vehicleSpawner,
            IStringLocalizer stringLocalizer,
            VehicleShopModel vehicleShopModel)
        {
            _economyProvider = economyProvider;
            _vehicleDirectory = vehicleDirectory;
            _vehicleSpawner = vehicleSpawner;
            _stringLocalizer = stringLocalizer;

            ShopData = vehicleShopModel;
        }

        private async Task<IVehicleAsset> GetVehicleAsset()
        {
            var id = ShopData.VehicleId.ToString();

            var vehicleAsset =
                (await _vehicleDirectory.GetVehicleAssetsAsync()).FirstOrDefault(x => x.VehicleAssetId.Equals(id));

            if (vehicleAsset == null)
                throw new Exception($"Vehicle id '{id}' has no asset");

            return vehicleAsset;
        }

        public async Task<decimal> Buy(IPlayerUser user)
        {
            var vehicleAsset = await GetVehicleAsset();

            var balance = await _economyProvider.UpdateBalanceAsync(user.Id, user.Type, -ShopData.BuyPrice,
                _stringLocalizer["transactions:vehicles:bought",
                    new
                    {
                        VehicleAsset = vehicleAsset,
                        Price = ShopData.BuyPrice,
                        _economyProvider.CurrencySymbol,
                        _economyProvider.CurrencyName
                    }]);
            
            // todo: Switch to non extension method in next update
            await _vehicleSpawner.SpawnVehicleAsync(user.Player, vehicleAsset.VehicleAssetId);

            return balance;
        }
    }
}
