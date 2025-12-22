namespace ECommerceModule.Contract.Catalog.Commands;

public record DeleteProductCommand(int ProductId) : ICommand<bool>;
