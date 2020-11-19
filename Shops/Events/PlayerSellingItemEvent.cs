using OpenMod.Unturned.Users;

namespace Shops.Events
{
    public class PlayerSellingItemEvent : PlayerSellingEvent
    {
        public PlayerSellingItemEvent(UnturnedUser user, ushort id, int amount, decimal unitPrice) : base(user, id, amount, unitPrice)
        {
        }
    }
}
