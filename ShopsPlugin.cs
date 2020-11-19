using Cysharp.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using OpenMod.API.Commands;
using OpenMod.API.Plugins;
using OpenMod.EntityFrameworkCore.Extensions;
using OpenMod.Unturned.Plugins;
using SDG.Unturned;
using Shops.Database;
using Shops.Database.Models;
using Shops.Shops;
using System;
using System.Linq;
using System.Threading.Tasks;

[assembly: PluginMetadata("Shops", Author = "SilK", DisplayName = "Shops")]
namespace Shops
{
    public class ShopsPlugin : OpenModUnturnedPlugin
    {
        private readonly ShopDbContext m_DbContext;
        private readonly ILogger<ShopsPlugin> m_Logger;
        private readonly IConfiguration m_Configuration;
        private readonly IStringLocalizer m_StringLocalizer;
        private readonly IServiceProvider m_ServiceProvider;

        public ShopsPlugin(
            ShopDbContext dbContext,
            ILogger<ShopsPlugin> logger,
            IConfiguration configuration,
            IStringLocalizer stringLocalizer,
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            m_DbContext = dbContext;
            m_Logger = logger;
            m_Configuration = configuration;
            m_StringLocalizer = stringLocalizer;
            m_ServiceProvider = serviceProvider;
        }

        public bool CanBuyItems => m_Configuration.GetSection("Shops:CanBuyItems").Get<bool>();
        public bool CanSellItems => m_Configuration.GetSection("Shops:CanSellItems").Get<bool>();
        public bool CanBuyVehicles => m_Configuration.GetSection("Shops:CanBuyVehicles").Get<bool>();
        public bool CanSellVehicles => m_Configuration.GetSection("Shops:CanSellVehicles").Get<bool>();
        public bool QualityCounts => m_Configuration.GetSection("Shops:QualityCounts").Get<bool>();

        protected override async UniTask OnLoadAsync()
        {
            await m_DbContext.OpenModMigrateAsync();

            m_Logger.LogInformation(m_StringLocalizer["shops:logs:plugin_loaded"]);
        }

        protected override async UniTask OnUnloadAsync()
        {
            m_Logger.LogInformation(m_StringLocalizer["shops:logs:plugin_unloaded"]);
        }

        public Asset GetAsset(EAssetType type, string idOrName)
        {
            Asset asset;

            if (ushort.TryParse(idOrName, out ushort id))
            {
                asset = Assets.find(type, id);
            }
            else
            {
                Asset[] assets = Assets.find(type);

                switch (type)
                {
                    case EAssetType.ITEM:
                        asset = assets.OfType<ItemAsset>().FirstOrDefault(x => x.itemName != null && x.itemName.ToLower().Contains(idOrName.ToLower()));
                        break;
                    case EAssetType.VEHICLE:
                        asset = assets.OfType<VehicleAsset>().FirstOrDefault(x => x.vehicleName != null && x.vehicleName.ToLower().Contains(idOrName.ToLower()));
                        break;
                    default:
                        asset = assets.FirstOrDefault(x => x.name != null && x.name.ToLower().Contains(idOrName.ToLower()));
                        break;
                }
            }

            if (asset == null)
            {
                m_Logger.LogDebug(m_StringLocalizer["shops:logs:asset_not_found", new { IdOrName = idOrName }]);
            }

            return asset;
        }

        public async Task<ShopBuyItem> GetBuyItemShop(ushort id)
        {
            BuyItem shop = await m_DbContext.BuyItemShops.FindAsync((int)id);

            if (shop == null) return null;

            return ActivatorUtilities.CreateInstance<ShopBuyItem>(m_ServiceProvider, shop);
        }

        public async Task<ShopSellItem> GetSellItemShop(ushort id)
        {
            SellItem shop = await m_DbContext.SellItemShops.FindAsync((int)id);

            if (shop == null) return null;

            return ActivatorUtilities.CreateInstance<ShopSellItem>(m_ServiceProvider, shop);
        }

        public async Task<ShopBuyVehicle> GetBuyVehicleShop(ushort id)
        {
            BuyVehicle shop = await m_DbContext.BuyVehicleShops.FindAsync((int)id);

            if (shop == null) return null;

            return ActivatorUtilities.CreateInstance<ShopBuyVehicle>(m_ServiceProvider, shop);
        }

        public void AssertCanBuyItems()
        {
            if (!CanBuyItems)
            {
                throw new UserFriendlyException(m_StringLocalizer["shops:disabled:item_buy"]);
            }
        }

        public void AssertCanSellItems()
        {
            if (!CanSellItems)
            {
                throw new UserFriendlyException(m_StringLocalizer["shops:disabled:item_sell"]);
            }
        }

        public void AssertCanBuyVehicles()
        {
            if (!CanBuyVehicles)
            {
                throw new UserFriendlyException(m_StringLocalizer["shops:disabled:vehicle_buy"]);
            }
        }

        public void AssertCanSellVehicles()
        {
            if (!CanSellVehicles)
            {
                throw new UserFriendlyException(m_StringLocalizer["shops:disabled:vehicle_sell"]);
            }
        }
    }
}
