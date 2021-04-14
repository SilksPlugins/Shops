using Autofac;
using Microsoft.Extensions.DependencyInjection;
using OpenMod.API.Ioc;
using OpenMod.API.Plugins;
using OpenMod.API.Prioritization;
using OpenMod.Core.Ioc;
using Shops.API;
using Shops.API.Items;
using Shops.API.Vehicles;
using Shops.Database;
using Shops.Database.Models;
using Shops.Shops.Items;
using Shops.Shops.Vehicles;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Shops.Shops
{
    [ServiceImplementation(Lifetime = ServiceLifetime.Singleton, Priority = Priority.Lowest)]
    public class ShopManager : IShopManager
    {
        private readonly IPluginAccessor<ShopsPlugin> _pluginAccessor;

        public ShopManager(
            IPluginAccessor<ShopsPlugin> pluginAccessor)
        {
            _pluginAccessor = pluginAccessor;
        }

        private ILifetimeScope GetPluginScope() => _pluginAccessor.Instance?.LifetimeScope ??
                                                   throw new Exception("Shops plugin is not loaded");

        private ShopsDbContext? _cachedDbContext;

        private ShopsDbContext GetDbContext() => _cachedDbContext ??= GetPluginScope().Resolve<ShopsDbContext>();

        public IQueryable<IItemShopData> GetItemShopDatas() => GetDbContext().ItemShops.AsQueryable();

        public IQueryable<IVehicleShopData> GetVehicleShopDatas() => GetDbContext().VehicleShops.AsQueryable();

        public async Task<ItemShopModel?> GetItemShopData(string id) =>
            await GetDbContext().ItemShops.FindAsync(id);

        async Task<IItemShopData?> IShopManager.GetItemShopData(string id) =>
            await GetItemShopData(id);

        public async Task<VehicleShopModel?> GetVehicleShopData(string id) =>
            await GetDbContext().VehicleShops.FindAsync(id);

        async Task<IVehicleShopData?> IShopManager.GetVehicleShopData(string id) =>
            await GetVehicleShopData(id);

        public async Task<IItemShop?> GetItemShop(string id)
        {
            var data = await GetItemShopData(id);

            return data == null
                ? null
                : ActivatorUtilitiesEx.CreateInstance<ItemShop>(GetPluginScope(), data);
        }

        public async Task<IVehicleShop?> GetVehicleShop(string id)
        {
            var data = await GetVehicleShopData(id);

            return data == null
                ? null
                : ActivatorUtilitiesEx.CreateInstance<VehicleShop>(GetPluginScope(), data);
        }
        
        public async Task<IItemShop> AddItemShopBuyable(string id, decimal price)
        {
            var data = await GetItemShopData(id);

            var dbContext = GetDbContext();

            if (data == null)
            {
                data = new ItemShopModel
                {
                    ItemId = id,
                    BuyPrice = price
                };

                await dbContext.ItemShops.AddAsync(data);
            }
            else
            {
                data.BuyPrice = price;

                dbContext.ItemShops.Update(data);
            }

            await dbContext.SaveChangesAsync();

            return ActivatorUtilitiesEx.CreateInstance<ItemShop>(GetPluginScope(), data);
        }

        public async Task<IItemShop> AddItemShopSellable(string id, decimal price)
        {
            var data = await GetItemShopData(id);

            var dbContext = GetDbContext();

            if (data == null)
            {
                data = new ItemShopModel
                {
                    ItemId = id,
                    SellPrice = price
                };

                await dbContext.ItemShops.AddAsync(data);
            }
            else
            {
                data.SellPrice = price;

                dbContext.ItemShops.Update(data);
            }

            await dbContext.SaveChangesAsync();

            return ActivatorUtilitiesEx.CreateInstance<ItemShop>(GetPluginScope(), data);
        }

        public async Task<IVehicleShop> AddVehicleShopBuyable(string id, decimal price)
        {
            var data = await GetVehicleShopData(id);

            var dbContext = GetDbContext();

            if (data == null)
            {
                data = new VehicleShopModel
                {
                    VehicleId = id,
                    BuyPrice = price
                };

                await dbContext.VehicleShops.AddAsync(data);
            }
            else
            {
                data.BuyPrice = price;

                dbContext.VehicleShops.Update(data);
            }

            await dbContext.SaveChangesAsync();

            return ActivatorUtilitiesEx.CreateInstance<VehicleShop>(GetPluginScope(), data);
        }

        public async Task<bool> RemoveItemShopBuyable(string id)
        {
            var data = await GetItemShopData(id);

            var dbContext = GetDbContext();

            if (data?.BuyPrice == null) return false;

            data.BuyPrice = null;

            if (data.SellPrice == null)
            {
                dbContext.ItemShops.Remove(data);
            }
            else
            {
                dbContext.ItemShops.Update(data);
            }

            await dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RemoveItemShopSellable(string id)
        {
            var data = await GetItemShopData(id);

            var dbContext = GetDbContext();

            if (data?.SellPrice == null) return false;

            data.SellPrice = null;

            if (data.BuyPrice == null)
            {
                dbContext.ItemShops.Remove(data);
            }
            else
            {
                dbContext.ItemShops.Update(data);
            }

            await dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RemoveVehicleShopBuyable(string id)
        {
            var data = await GetVehicleShopData(id);

            var dbContext = GetDbContext();

            if (data == null) return false;

            dbContext.VehicleShops.Remove(data);

            await dbContext.SaveChangesAsync();

            return true;
        }
    }
}
