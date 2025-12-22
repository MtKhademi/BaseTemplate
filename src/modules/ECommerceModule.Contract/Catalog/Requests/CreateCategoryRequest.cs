namespace ECommerceModule.Contract.Catalog.Requests;

public record CreateCategoryRequest(
    string Name,
    int? ParentId
)
{
    public CreateCategoryCommand ToCommand() => CreateCategoryCommand.Create(this);
}
