using Cysharp.Threading.Tasks;
using OpenMod.Unturned.Users;

namespace Shops.Shops
{
    public interface IShop
    {
        UniTask Interact(UnturnedUser user, int amount);
    }
}
