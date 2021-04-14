using OpenMod.API.Prioritization;
using OpenMod.Core.Commands;
using System;
using System.Threading.Tasks;

namespace Shops.Commands.Items
{
    [Command("add", Priority = Priority.Normal)]
    [CommandAlias("a")]
    [CommandAlias("+")]
    [CommandSyntax("<buy | sell> <item> <price>")]
    [CommandParent(typeof(CShop))]
    [CommandDescription("Adds an item shop.")]
    public class CShopAdd : Command
    {
        public CShopAdd(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        protected override Task OnExecuteAsync()
        {
            throw new CommandWrongUsageException(Context);
        }
    }
}
