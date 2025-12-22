using Infrastructure.Pagination;

namespace ECommerceModule.Catalog.Extensions;

internal static class CatalogMappingExtensions
{
    public static ProductModel ToProductModel(this ProductEntity entity)
        => new(entity.Id, entity.Name, entity.Price, entity.IsActive, entity.CategoryId);

    public static ProductResponse ToProductResponse(this ProductModel model)
        => new(model.ProductId, model.Name, model.Price, model.IsActive, model.CategoryId);

    public static CategoryResponse ToCategoryResponse(this CategoryModel model)
        => new(model.CategoryId, model.Name, model.ParentId);

    public static PaginatedList<ProductResponse> ToProductResponsePaginated(this PaginatedList<ProductModel> list)
        => list.ToPaginatedList(x => x.ToProductResponse());

    public static PaginatedList<CategoryResponse> ToCategoryResponsePaginated(this PaginatedList<CategoryModel> list)
        => list.ToPaginatedList(x => x.ToCategoryResponse());
}
