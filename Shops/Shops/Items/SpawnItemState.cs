using OpenMod.Extensions.Games.Abstractions.Items;

namespace Shops.Shops.Items
{
    public class SpawnItemState : IItemState
    {
        public SpawnItemState(IItemAsset itemAsset)
        {
            // todo: in the OpenMod update, set to max asset values
            ItemQuality = 100;
            ItemDurability = 100;
            ItemAmount = 1;
            StateData = null;
        }

        public double ItemQuality { get; }
        public double ItemDurability { get; }
        public double ItemAmount { get; }
        public byte[]? StateData { get; }
    }
}
