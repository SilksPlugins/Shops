using OpenMod.Unturned.Users;
using OpenMod.Unturned.Users.Events;

namespace Shops.Events
{
    public abstract class PlayerSoldEvent : UnturnedUserEvent
    {
        public ushort Id { get; }

        public int Amount { get; }

        public decimal UnitPrice { get; }

        public decimal TotalPrice => Amount * UnitPrice;

        protected PlayerSoldEvent(UnturnedUser user, ushort id, int amount, decimal unitPrice) : base(user)
        {
            Id = id;
            Amount = amount;
            UnitPrice = unitPrice;
        }
    }
}
