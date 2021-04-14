using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using OpenMod.API.Commands;
using OpenMod.Core.Commands;
using OpenMod.Core.Ioc;
using OpenMod.Extensions.Economy.Abstractions;
using OpenMod.Extensions.Games.Abstractions.Items;
using OpenMod.Extensions.Games.Abstractions.Players;
using OpenMod.Extensions.Games.Abstractions.Vehicles;
using Shops.API;
using System;
using System.Threading.Tasks;

namespace Shops.Commands
{
    [DontAutoRegister]
    public abstract class ShopCommand : Command
    {
        protected readonly IConfiguration Configuration;
        protected readonly IStringLocalizer StringLocalizer;
        protected readonly IItemDirectory ItemDirectory;
        protected readonly IVehicleDirectory VehicleDirectory;
        protected readonly IShopManager ShopManager;
        protected readonly IEconomyProvider EconomyProvider;

        protected ShopCommand(
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            Configuration = serviceProvider.GetRequiredService<IConfiguration>();
            StringLocalizer = serviceProvider.GetRequiredService<IStringLocalizer>();
            ItemDirectory = serviceProvider.GetRequiredService<IItemDirectory>();
            VehicleDirectory = serviceProvider.GetRequiredService<IVehicleDirectory>();
            ShopManager = serviceProvider.GetRequiredService<IShopManager>();
            EconomyProvider = serviceProvider.GetRequiredService<IEconomyProvider>();
        }

        public bool CanBuyItems => Configuration.GetValue("Shops:CanBuyItems", true);

        public bool CanSellItems => Configuration.GetValue("Shops:CanSellItems", true);

        public bool CanBuyVehicles => Configuration.GetValue("Shops:CanBuyVehicles", true);

        protected void AssertCanBuyItems()
        {
            if (!CanBuyItems)
                throw new UserFriendlyException(StringLocalizer["commands:errors:no_buy_items"]);
        }

        protected void AssertCanSellItems()
        {
            if (!CanSellItems)
                throw new UserFriendlyException(StringLocalizer["commands:errors:no_sell_items"]);
        }

        protected void AssertCanBuyVehicles()
        {
            if (!CanBuyVehicles)
                throw new UserFriendlyException(StringLocalizer["commands:errors:no_buy_vehicles"]);
        }

        protected void ValidatePrice(decimal price)
        {
            if (price < 0)
                throw new UserFriendlyException(StringLocalizer["commands:errors:invalid_price", new {Price = price}]);
        }

        protected void ValidateAmount(int amount)
        {
            if (amount < 1)
                throw new UserFriendlyException(
                    StringLocalizer["commands:errors:invalid_amount", new { Amount = amount }]);
        }

        protected async Task<IItemAsset> GetItemAsset(int index)
        {
            var idOrName = await Context.Parameters.GetAsync<string>(index);

            if (string.IsNullOrWhiteSpace(idOrName))
                throw new UserFriendlyException(StringLocalizer["commands:errors:invalid_item_id", new { IdOrName = idOrName }]);

            return await ItemDirectory.FindByIdAsync(idOrName) ??
                   await ItemDirectory.FindByNameAsync(idOrName) ?? throw new UserFriendlyException(StringLocalizer[
                       "commands:errors:invalid_item_id",
                       new {IdOrName = idOrName}]);
        }

        protected async Task<IVehicleAsset> GetVehicleAsset(int index)
        {
            var idOrName = await Context.Parameters.GetAsync<string>(index);

            if (string.IsNullOrWhiteSpace(idOrName))
                throw new UserFriendlyException(StringLocalizer["commands:errors:invalid_vehicle_id", new { IdOrName = idOrName }]);

            return await VehicleDirectory.FindByIdAsync(idOrName)
                   ?? await VehicleDirectory.FindByNameAsync(idOrName, false)
                   ?? throw new UserFriendlyException(
                       StringLocalizer["commands:errors:invalid_vehicle_id", new {IdOrName = idOrName}]);
        }

        protected async Task<decimal> GetPrice(int index)
        {
            var price = await Context.Parameters.GetAsync<decimal>(index);
            ValidatePrice(price);
            return price;
        }

        protected async Task<int> GetAmount(int index)
        {
            var amount = await Context.Parameters.GetAsync<int>(index, 1);
            ValidateAmount(amount);
            return amount;
        }

        protected IPlayerUser GetPlayerUser() => (IPlayerUser)Context.Actor;
    }
}
