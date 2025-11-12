namespace MDF.Test.Integration.Modules.AnnouncementModuleTest.Responses;

public record DividendPaymentScheduleDetailResponseTest(
    int? DividendPaymentScheduleIdFk,
    short? ShareHolderType,
    string? FromDate,
    string? ToDate,
    long? FromCount,
    long? ToCount,
    string? FromChar,
    string? ToChar,
    int? Id
);