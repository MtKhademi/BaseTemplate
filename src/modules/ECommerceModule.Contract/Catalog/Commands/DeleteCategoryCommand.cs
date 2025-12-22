namespace ECommerceModule.Contract.Catalog.Commands;

public record DeleteCategoryCommand : ICommand<bool>
{
    public int CategoryId { get; init; }

    public DeleteCategoryCommand(int categoryId)
    {
        var errors = new List<string>();
        if (categoryId <= 0)
        {
            errors.Add("CategoryId must be greater than zero.");
        }
        if (errors.Any())
        {
            throw new DeleteCategoryCommandException(errors);
        }

        CategoryId = categoryId;
    }
    internal class DeleteCategoryCommandException : NotValidDataException<DeleteCategoryCommandException>
    {
        public DeleteCategoryCommandException(IEnumerable<string> errors)
            : base(errors)
        {
        }
    }

}
