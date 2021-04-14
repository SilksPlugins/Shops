using OpenMod.API.Ioc;
using Shops.API.Items;
using Shops.API.Vehicles;
using System.Linq;
using System.Threading.Tasks;

namespace Shops.API
{
    [Service]
    public interface IShopManager
    {
        IQueryable<IItemShopData> GetItemShopDatas();
        IQueryable<IVehicleShopData> GetVehicleShopDatas();

        Task<IItemShopData?> GetItemShopData(string id);
        Task<IVehicleShopData?> GetVehicleShopData(string id);

        Task<IItemShop?> GetItemShop(string id);
        Task<IVehicleShop?> GetVehicleShop(string id);

        Task<IItemShop> AddItemShopBuyable(string id, decimal price);
        Task<IItemShop> AddItemShopSellable(string id, decimal price);
        Task<IVehicleShop> AddVehicleShopBuyable(string id, decimal price);

        Task<bool> RemoveItemShopBuyable(string id);
        Task<bool> RemoveItemShopSellable(string id);
        Task<bool> RemoveVehicleShopBuyable(string id);
    }
}
