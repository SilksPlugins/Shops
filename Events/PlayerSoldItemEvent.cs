using OpenMod.Unturned.Users;

namespace Shops.Events
{
    public class PlayerSoldItemEvent : PlayerSoldEvent
    {
        public PlayerSoldItemEvent(UnturnedUser user, ushort id, int amount, decimal unitPrice) : base(user, id, amount, unitPrice)
        {
        }
    }
}
