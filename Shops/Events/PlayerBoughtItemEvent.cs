using OpenMod.Unturned.Users;

namespace Shops.Events
{
    public class PlayerBoughtItemEvent : PlayerBoughtEvent
    {
        public PlayerBoughtItemEvent(UnturnedUser user, ushort id, int amount, decimal unitPrice) : base(user, id, amount, unitPrice)
        {
        }
    }
}
