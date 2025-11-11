namespace MDF.Test.Integration.Modules.AnnouncementModuleTest.Responses;

public record RightDurationDateResponseTest(
    int? RightDurationDateId,
    string? StartDate,
    string? EndDate,
    int? Code,
    string? InsertedBy,
    string? EntryTime,
    string? ModifiedTime,
    int? AnnouncementId,
    int? MeetingAnnouncementId,
    bool? IsTraded,
    byte? CalculationMethod,
    int? FirmId,
    int? Period,
    decimal? Price,
    string? Description
);