using ECommerceModule.Contract.Catalog.Commands;

namespace ECommerceModule.Catalog.Features.UpdateCategory;

internal class UpdateCategoryHandler(ICategoryRepository repository)
    : ICommandHandler<UpdateCategoryCommand, CategoryModel>
{
    public async Task<CategoryModel> Handle(UpdateCategoryCommand command, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIDAsync(command.CategoryId)
            ?? throw new EntityNotFoundException<CategoryEntity, int>(command.CategoryId);

        entity.Update(command.Name, command.ParentId);
        await repository.UpdateAsync(entity);

        return entity.ToCategoryModel();
    }
}
