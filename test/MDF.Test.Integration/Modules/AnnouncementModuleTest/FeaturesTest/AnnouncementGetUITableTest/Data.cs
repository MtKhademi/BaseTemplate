using MDF.Test.Integration.Modules.AnnouncementModuleTest.Requests;

namespace MDF.Test.Integration.Modules.AnnouncementModuleTest.AnnouncementTest.AnnouncementGetUITableTest;

internal class AnnouncementGetPaginatedListRequestTestNotValidData : TheoryData<AnnouncementGetPaginatedListRequestTest, List<string>>
{
    public AnnouncementGetPaginatedListRequestTestNotValidData()
    {
        Add(new AnnouncementGetPaginatedListRequestTest
        {
            StartDateTime = "123"
        },
            ["Can  not convert this 123 to dateTime =\u003E TEMPLATE : yyyy-mm-ddThh:mm"]);
    }
}
