namespace MDF.Test.Integration.Modules.MutualFundModuleTest.OrganizationsTest.FeaturesTest.OrganizationAddTest;

internal class OrganizationAddDtoNotValidData : TheoryData<OrganizationAddDtoV4Test, List<string>>
{
    public OrganizationAddDtoNotValidData()
    {
        var dto = new OrganizationAddDtoV4Test(OrganizationTypeId: 0, NationalCode: null,
            RegisterNumber: null, RegisterDate: null, Title: null);
        Add(dto, [
            "National code is required",
            "Register number is required",
            "Register Date can not be convert to date =\u003E sample YYYY-MM-DD :: ",
            "Title is required",
            "Organization type id is required"
            ]);
    }
}
