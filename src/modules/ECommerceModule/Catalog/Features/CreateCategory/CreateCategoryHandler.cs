using ECommerceModule.Contract.Catalog.Commands;

namespace ECommerceModule.Catalog.Features.CreateCategory;

internal class CreateCategoryHandler(ICategoryRepository repository)
    : ICommandHandler<CreateCategoryCommand, CategoryModel>
{
    public async Task<CategoryModel> Handle(CreateCategoryCommand command, CancellationToken cancellationToken)
    {
        var entity = new CategoryEntity(command.Name, command.ParentId);
        await repository.CreateAsync(entity, cancellationToken);
        return entity.ToCategoryModel();
    }
}
