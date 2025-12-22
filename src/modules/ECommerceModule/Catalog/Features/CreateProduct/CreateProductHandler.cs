using ECommerceModule.Contract.Catalog.Commands;

namespace ECommerceModule.Catalog.Features.CreateProduct;

internal class CreateProductHandler(IProductRepository repository)
    : ICommandHandler<CreateProductCommand, ProductModel>
{
    public async Task<ProductModel> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        var entity = new ProductEntity(command.Name, command.Price, command.CategoryId, command.IsActive);
        await repository.CreateAsync(entity, cancellationToken);
        return entity.ToProductModel();
    }
}
