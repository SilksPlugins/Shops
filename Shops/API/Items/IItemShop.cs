using OpenMod.Extensions.Games.Abstractions.Players;
using System.Threading.Tasks;

namespace Shops.API.Items
{
    public interface IItemShop
    {
        IItemShopData ShopData { get; }
        
        bool CanBuy();
        Task<decimal> Buy(IPlayerUser user, int amount = 1);

        bool CanSell();
        Task<decimal> Sell(IPlayerUser user, int amount = 1);
    }
}
