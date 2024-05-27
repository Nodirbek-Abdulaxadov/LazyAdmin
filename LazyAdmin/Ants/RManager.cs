using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace LazyAdmin.Ants;

public class RManager<TEntity> where TEntity : class
{
    public static List<INavigation> HasOneToManyRelation(DbContext context)
    {
        var entityType = context.Model.FindEntityType(typeof(TEntity));
        if (entityType == null)
        {
            throw new ArgumentException($"Entity type {typeof(TEntity).Name} not found in the context.");
        }

        return entityType.GetNavigations()
            .Where(n => n.IsCollection).ToList();
    }

    public static List<INavigation> HasManyToOneRelation(DbContext context)
    {
        var entityType = context.Model.FindEntityType(typeof(TEntity));
        if (entityType == null)
        {
            throw new ArgumentException($"Entity type {typeof(TEntity).Name} not found in the context.");
        }

        return entityType.GetNavigations()
            .Where(n => !n.IsCollection).ToList();
    }

}