using OpenMod.Extensions.Games.Abstractions.Players;
using System.Threading.Tasks;

namespace Shops.API.Vehicles
{
    public interface IVehicleShop
    {
        IVehicleShopData ShopData { get; }
        
        Task<decimal> Buy(IPlayerUser user);
    }
}
