using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repository;

internal class AnnotationHelper
{
    private static string GetName(IEntityType entityType,
                              string defaultSchemaName = "dbo")
    {
        var schema = entityType.FindAnnotation("Relational:Schema").Value;
        string tableName = entityType.GetAnnotation
                           ("Relational:TableName").Value.ToString();
        string schemaName = schema == null ? defaultSchemaName : schema.ToString();
        /*table full name*/
        string name = string.Format("[{0}].[{1}]", schemaName, tableName);
        return name;
    }

    public static string TableName<T>(DbContext dbContext) where T : class
    {
        var entityType = dbContext.Model.FindEntityType(typeof(T));
        return GetName(entityType);
    }

    public static string TableName<T>(DbSet<T> dbSet) where T : class
    {
        var entityType = dbSet.EntityType;
        return GetName(entityType);
    }
}
