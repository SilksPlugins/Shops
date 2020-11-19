using Cysharp.Threading.Tasks;
using OpenMod.Core.Commands;
using OpenMod.Unturned.Commands;
using SDG.Unturned;
using System;

namespace Shops.Commands.Items
{
    [Command("remove")]
    [CommandAlias("r")]
    [CommandAlias("rem")]
    [CommandAlias("-")]
    [CommandSyntax("<buy/sell> <id or name>")]
    [CommandParent(typeof(CShopItem))]
    public class CShopItemRemove : UnturnedCommand
    {
        public CShopItemRemove(IServiceProvider serviceProvider) : base(serviceProvider)
        {

        }

        protected override UniTask OnExecuteAsync()
        {
            throw new CommandWrongUsageException(Context);
        }
    }
}
