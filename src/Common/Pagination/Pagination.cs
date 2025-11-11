namespace Common.Pagination;

public interface IPagination
{
    int CurrentPage { get; set; }
    int PageSize { get; set; }
}

public class Pagination : IPagination
{
    public int CurrentPage { get; set; } = 1;
    public int PageSize { get; set; } = 50;

    public int Skip => (CurrentPage - 1) * PageSize;
    public int Take => PageSize;

    public void Validate(ICollection<string> errors)
    {
        if (CurrentPage < 1)
            errors.Add($"{nameof(CurrentPage)} must be greater than or equal to 1.");
        if (PageSize < 1 || PageSize > 1000)
            errors.Add($"{nameof(PageSize)} must be between 1 and 1000.");
    }
}
