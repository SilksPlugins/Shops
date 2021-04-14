using OpenMod.Extensions.Games.Abstractions.Items;
using OpenMod.Extensions.Games.Abstractions.Players;
using System;

namespace Shops.Helpers
{
    public static class PlayerExtensions
    {
        public static IInventory? GetInventory(this IPlayer player)
        {
            // ReSharper disable once SuspiciousTypeConversion.Global
            if (player is not IHasInventory hasInventory)
                throw new Exception("Player not supported - does not have inventory");

            return hasInventory.Inventory;
        }
    }
}
