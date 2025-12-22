namespace ECommerceModule.Catalog.Extensions;

internal static class CatalogQueryExtensions
{
    public static IQueryable<ProductEntity> FilterByCategory(this IQueryable<ProductEntity> query, int? categoryId)
        => categoryId.HasValue && categoryId.Value > 0 ? query.Where(x => x.CategoryId == categoryId.Value) : query;

    public static IQueryable<ProductEntity> FilterByActive(this IQueryable<ProductEntity> query, bool? isActive)
        => isActive.HasValue ? query.Where(x => x.IsActive == isActive.Value) : query;
}
