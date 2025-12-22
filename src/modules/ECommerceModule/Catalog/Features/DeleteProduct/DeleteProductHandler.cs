using ECommerceModule.Contract.Catalog.Commands;

namespace ECommerceModule.Catalog.Features.DeleteProduct;

internal class DeleteProductHandler(IProductRepository repository)
    : ICommandHandler<DeleteProductCommand, bool>
{
    public async Task<bool> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIDAsync(command.ProductId)
            ?? throw new EntityNotFoundException<ProductEntity, int>(command.ProductId);

        await repository.DeleteHardAsync(entity);
        return true;
    }
}
