using Cysharp.Threading.Tasks;
using OpenMod.Core.Commands;
using OpenMod.Unturned.Commands;
using System;

namespace Shops.Commands.Items
{
    [Command("shop")]
    [CommandAlias("shopi")]
    [CommandAlias("ishop")]
    [CommandAlias("shopitem")]
    [CommandAlias("itemshop")]
    [CommandDescription("Manages item shops")]
    [CommandSyntax("<[a]dd/[r]emove/[c]hange>")]
    public class CShopItem : UnturnedCommand
    {
        public CShopItem(IServiceProvider serviceProvider) : base(serviceProvider)
        {

        }

        protected override UniTask OnExecuteAsync()
        {
            throw new CommandWrongUsageException(Context);
        }
    }
}
