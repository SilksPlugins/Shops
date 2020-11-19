using OpenMod.Unturned.Users;

namespace Shops.Events
{
    public class PlayerBuyingVehicleEvent : PlayerBuyingEvent
    {
        public PlayerBuyingVehicleEvent(UnturnedUser user, ushort id, decimal unitPrice) : base(user, id, 1, unitPrice)
        {
        }
    }
}
