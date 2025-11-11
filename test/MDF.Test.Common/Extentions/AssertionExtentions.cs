using MDF.Common.Infrastructure.TableInfra.TableInfra;

namespace MDF.Test.Common.Extentions;

public static class AssertionExtentions
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


    public static void AssertionUITableEmpty<TRow>(this BaseTableDto<TRow> tableDto, int countColumn, int pageNumber = 0, int pageSize = 50)
    {
        //- check basic------------------------------------------
        tableDto.Metadata.Basic.PersianTitle.Should().NotBeNullOrWhiteSpace();
        tableDto.Metadata.Basic.HasRowNumber.Should().BeTrue();
        tableDto.Data.Should().NotBeNull();
        tableDto.Data.Pageable.Should().NotBeNull();
        tableDto.Data.Content.Should().NotBeNull();
        tableDto.Data.Content.Should().HaveCount(0);
        tableDto.Metadata.Details.Should().NotBeNull();


        //-check columns---------------------------------------
        tableDto.Metadata.Details.Count.Should().Be(countColumn);

        tableDto.Data.Pageable.PageNumber.Should().Be(pageNumber);
        tableDto.Data.Pageable.PageSize.Should().Be(pageSize);
    }

    public static void AssertionUITable<TRow>(this BaseTableDto<TRow> tableDto, int countColumn, int pageNumber = 0, int pageSize = 50)
    {
        //- check basic------------------------------------------
        tableDto.Metadata.Basic.PersianTitle.Should().NotBeNullOrWhiteSpace();
        tableDto.Metadata.Basic.HasRowNumber.Should().BeTrue();
        tableDto.Data.Should().NotBeNull();
        tableDto.Data.Pageable.Should().NotBeNull();
        tableDto.Data.Content.Should().NotBeNull();
        tableDto.Metadata.Details.Should().NotBeNull();


        //-check columns---------------------------------------
        tableDto.Metadata.Details.Count.Should().Be(countColumn);

        tableDto.Data.Pageable.PageNumber.Should().Be(pageNumber);
        tableDto.Data.Pageable.PageSize.Should().Be(pageSize);
    }


    public static List<DetailInformation> ColumnAssertion(this List<DetailInformation> columns, string columnName, string columnDataType = "String",
        string persianTitle = null)
    {

        var columnExist = columns.FirstOrDefault(col => col.Name == columnName.ToCamaleCase());
        columnExist.Should().NotBeNull($"{columnName} not exist in columns");

        columnExist.PersianTitle.Should().NotBeNullOrWhiteSpace($"persianTitle for {columnName} don't set");
        if (persianTitle is not null) columnExist.PersianTitle.Should().Be(persianTitle, $"persianTitle for {columnName} must be {persianTitle} ");

        columnExist.DataType.Should().Be(columnDataType, $"{columnName} must be type of : {columnDataType}, but it's {columnExist.DataType}");

        return columns;

    }

    public static IList<string> ActoinAssertion(this IList<string> actions, string actionName)
    {

        actions.Should().Contain(actionName, $"Dosen't exist action : {actionName}");
        return actions;
    }

    public static IList<string> ActoinAssertion(this IList<string> actions, IList<string>? actionNames)
    {
        if (actionNames == null || !actionNames.Any())
            actions.Should().BeEmpty();
        else
            actions.Should().Contain(actionNames);

        return actions;
    }

    public static void AssertionIfExistDataThenHaveToNull(this string? value, string? data = null)
    {
        if (string.IsNullOrEmpty(data))
            value.Should().BeNull();
        else
            value.Should().Be(data);
    }

}
