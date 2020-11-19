using OpenMod.Unturned.Users;

namespace Shops.Events
{
    public class PlayerBoughtVehicleEvent : PlayerBoughtEvent
    {
        public PlayerBoughtVehicleEvent(UnturnedUser user, ushort id, decimal unitPrice) : base(user, id, 1, unitPrice)
        {
        }
    }
}
