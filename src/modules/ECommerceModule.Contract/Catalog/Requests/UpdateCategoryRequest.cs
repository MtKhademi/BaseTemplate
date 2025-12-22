namespace ECommerceModule.Contract.Catalog.Requests;

public record UpdateCategoryRequest(
    int CategoryId,
    string Name,
    int? ParentId
)
{
    public UpdateCategoryCommand ToCommand() => UpdateCategoryCommand.Create(this);
}
