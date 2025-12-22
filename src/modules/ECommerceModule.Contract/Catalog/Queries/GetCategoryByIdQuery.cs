namespace ECommerceModule.Contract.Catalog.Queries;

public record GetCategoryByIdQuery : IQuery<CategoryModel>
{
    public int CategoryId { get; init; }
    public GetCategoryByIdQuery(int categoryId)
    {
        if (categoryId <= 0)
        {
            throw new GetCategoryByIdQueryException("Category ID must be greater than zero.");
        }

        CategoryId = categoryId;
    }

    internal class GetCategoryByIdQueryException :
        NotValidDataException<GetCategoryByIdQueryException>
    {
        public GetCategoryByIdQueryException(string message) : base(message)
        {
        }
    }
}
