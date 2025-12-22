using ECommerceModule.Contract.Catalog.Commands;

namespace ECommerceModule.Catalog.Features.DeleteCategory;

internal class DeleteCategoryHandler(ICategoryRepository repository)
    : ICommandHandler<DeleteCategoryCommand, bool>
{
    public async Task<bool> Handle(DeleteCategoryCommand command, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIDAsync(command.CategoryId)
            ?? throw new EntityNotFoundException<CategoryEntity, int>(command.CategoryId);

        await repository.DeleteHardAsync(entity);
        return true;
    }
}
