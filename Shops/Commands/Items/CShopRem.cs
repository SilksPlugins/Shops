using OpenMod.Core.Commands;
using System;
using System.Threading.Tasks;

namespace Shops.Commands.Items
{
    [Command("rem")]
    [CommandAlias("r")]
    [CommandAlias("remove")]
    [CommandAlias("-")]
    [CommandSyntax("<buy | sell> <item>")]
    [CommandParent(typeof(CShop))]
    [CommandDescription("Removes an item shop.")]
    public class CShopRem : Command
    {
        public CShopRem(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        protected override Task OnExecuteAsync()
        {
            throw new CommandWrongUsageException(Context);
        }
    }
}