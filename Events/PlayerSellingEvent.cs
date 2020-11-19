using OpenMod.API.Eventing;
using OpenMod.Unturned.Users;
using OpenMod.Unturned.Users.Events;

namespace Shops.Events
{
    public abstract class PlayerSellingEvent : UnturnedUserEvent, ICancellableEvent
    {
        public ushort Id { get; set; }

        public int Amount { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal TotalPrice => Amount * UnitPrice;

        public bool IsCancelled { get; set; }

        protected PlayerSellingEvent(UnturnedUser user, ushort id, int amount, decimal unitPrice) : base(user)
        {
            Id = id;
            Amount = amount;
            UnitPrice = unitPrice;
        }
    }
}
