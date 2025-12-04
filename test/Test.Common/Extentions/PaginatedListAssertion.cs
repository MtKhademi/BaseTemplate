namespace Test.Infrastructure.Extentions;

public static class PaginatedListAssertion
{
    public static void AssertionPaginatedListEmpty<TData>(this PaginatedListTest<TData> paginatedList)
        where TData : class
    {
        ////- check basic------------------------------------------
        paginatedList.CurrentPage.Should().Be(0);
        paginatedList.SizeOfPage.Should().Be(50);
        paginatedList.TotalItems.Should().Be(0);
        paginatedList.TotalPages.Should().Be(0);
        paginatedList.Data.Should().NotBeNull();
        paginatedList.Data.Should().HaveCount(0);
        paginatedList.HasPreviousPage.Should().BeFalse();
        paginatedList.HasNextPage.Should().BeFalse();

    }
}
