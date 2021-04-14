using OpenMod.API.Prioritization;
using OpenMod.Core.Commands;
using System;
using System.Threading.Tasks;

namespace Shops.Commands.Items
{
    [Command("shop", Priority = Priority.Normal)]
    [CommandAlias("shops")]
    [CommandAlias("ishop")]
    [CommandAlias("ishops")]
    [CommandAlias("itemshop")]
    [CommandAlias("itemshops")]
    [CommandSyntax("<add | rem>")]
    [CommandDescription("Manages item shops.")]
    public class CShop : Command
    {
        public CShop(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        protected override Task OnExecuteAsync()
        {
            throw new CommandWrongUsageException(Context);
        }
    }
}
