using OpenMod.Unturned.Users;

namespace Shops.Events
{
    public class PlayerBuyingItemEvent : PlayerBuyingEvent
    {
        public PlayerBuyingItemEvent(UnturnedUser user, ushort id, int amount, decimal unitPrice) : base(user, id, amount, unitPrice)
        {
        }
    }
}
