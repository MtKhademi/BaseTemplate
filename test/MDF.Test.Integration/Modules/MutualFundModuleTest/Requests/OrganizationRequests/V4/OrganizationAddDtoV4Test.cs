namespace MDF.Test.Integration.Modules.MutualFundModuleTest.Dtos.OrganizationDtos.V4;

public record OrganizationAddDtoV4Test(
    byte OrganizationTypeId,
    string NationalCode,
    string RegisterNumber,
    string RegisterDate,
    string Title
);
