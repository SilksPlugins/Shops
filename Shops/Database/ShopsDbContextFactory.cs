using JetBrains.Annotations;
using OpenMod.EntityFrameworkCore.MySql;

namespace Shops.Database
{
    [UsedImplicitly]
    public class ShopsDbContextFactory : OpenModMySqlDbContextFactory<ShopsDbContext>
    {
    }
}
