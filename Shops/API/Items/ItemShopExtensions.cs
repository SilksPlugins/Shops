using OpenMod.Extensions.Games.Abstractions.Players;

namespace Shops.API.Items
{
    public static class ItemShopExtensions
    {
        public static bool TryBuy(this IItemShop shop, IPlayerUser player, int amount = 1)
        {
            if (shop.CanBuy()) return false;

            shop.Buy(player, amount);

            return true;
        }

        public static bool TrySell(this IItemShop shop, IPlayerUser player, int amount = 1)
        {
            if (shop.CanSell()) return false;

            shop.Sell(player, amount);

            return true;
        }
    }
}
