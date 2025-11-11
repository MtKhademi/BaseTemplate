namespace MDF.Test.Integration.Modules.AnnouncementModuleTest.Responses;

public record DividendPaymentScheduleResponseTest(
    int? Id,
    int? DividendAnnouncementIdFk,
    int? AnnouncementIdFk,
    string? Description,
    int? DividendAnnouncementType,
    ICollection<DividendPaymentScheduleDetailResponseTest>? DividendPaymentScheduleDetails
);