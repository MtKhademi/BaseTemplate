using ECommerceModule.Contract.Catalog.Commands;

namespace ECommerceModule.Catalog.Features.UpdateProduct;

internal class UpdateProductHandler(IProductRepository repository)
    : ICommandHandler<UpdateProductCommand, ProductModel>
{
    public async Task<ProductModel> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIDAsync(command.ProductId)
            ?? throw new EntityNotFoundException<ProductEntity, int>(command.ProductId);

        entity.Update(command.Name, command.Price, command.CategoryId, command.IsActive);
        await repository.UpdateAsync(entity);

        return entity.ToProductModel();
    }
}
