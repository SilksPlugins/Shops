using Cysharp.Threading.Tasks;
using OpenMod.Core.Commands;
using OpenMod.Unturned.Commands;
using SDG.Unturned;
using System;

namespace Shops.Commands.Items
{
    [Command("add")]
    [CommandAlias("a")]
    [CommandAlias("+")]
    [CommandSyntax("<buy/sell> <id or name> <cost>")]
    [CommandParent(typeof(CShopItem))]
    public class CShopItemAdd : UnturnedCommand
    {
        public CShopItemAdd(IServiceProvider serviceProvider) : base(serviceProvider)
        {

        }

        protected override UniTask OnExecuteAsync()
        {
            throw new CommandWrongUsageException(Context);
        }
    }
}
