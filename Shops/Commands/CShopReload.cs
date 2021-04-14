using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using OpenMod.API.Prioritization;
using OpenMod.Core.Commands;
using Shops.Commands.Items;
using Shops.Database;
using System;
using System.Threading.Tasks;

namespace Shops.Commands
{
    [Command("reload", Priority = Priority.High)]
    [CommandDescription("Reloads the shops from the database.")]
    [CommandParent(typeof(CShop))]
    public class CShopReload : Command
    {
        private readonly ShopsDbContext _dbContext;
        private readonly IStringLocalizer _stringLocalizer;

        public CShopReload(ShopsDbContext dbContext,
            IStringLocalizer stringLocalizer,
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _dbContext = dbContext;
            _stringLocalizer = stringLocalizer;
        }

        protected override async Task OnExecuteAsync()
        {
            await _dbContext.ItemShops.LoadAsync();
            await _dbContext.VehicleShops.LoadAsync();

            await PrintAsync(_stringLocalizer["commands:success:shop_reloaded"]);
        }
    }
}
