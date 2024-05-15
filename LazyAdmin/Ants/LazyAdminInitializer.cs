using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;

namespace LazyAdmin.Ants;

public class LazyAdminInitializer<T> where T : DbContext
{
    private static List<IEntityType> entityTypes = [];

    public static void Initialize(T context)
    {
        entityTypes = context.Model.GetEntityTypes().ToList();

        new Generator().GenerateLazyAdmin(entityTypes, context);
    }
}