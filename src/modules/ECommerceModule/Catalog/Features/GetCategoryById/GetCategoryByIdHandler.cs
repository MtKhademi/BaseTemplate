namespace ECommerceModule.Catalog.Features.GetCategoryById;

internal class GetCategoryByIdHandler(ICategoryRepository repository)
    : IQueryHandler<GetCategoryByIdQuery, CategoryModel>
{
    public async Task<CategoryModel> Handle(GetCategoryByIdQuery query, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIDAsync(query.CategoryId)
            ?? throw new EntityNotFoundException<CategoryEntity, int>(query.CategoryId);
        return entity.ToCategoryModel();
    }
}
