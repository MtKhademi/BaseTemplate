using AnnouncementModule.Contract.Responses;

namespace MDF.Test.Integration.Modules.AnnouncementModuleTest.Responses;

public class AnnouncementResponseTest
{
    public int? AnnouncementId { get; set; }
    public int? CodalCode { get; set; }
    public int? FirmId { get; set; }
    public string? SymbolIsin { get; set; }
    public string? SymbolName { get; set; }
    public string? OrganizationName { get; set; }
    public bool? IsConfirm { get; set; }
    public string? Title { get; set; }
    public string? TypeOfAnnouncement { get; set; }
    public string? PublishDate { get; set; }
    public string? ConfirmedName { get; set; }
    public string? ConfirmDateTime { get; set; }
    public string? HtmlLink { get; set; }

    public CapitalChangeResponseTest? CapitalChange { get; set; }
    public DividendPerShareResponseTest? DividendPerShare { get; set; }
    public RightDurationDateResponseTest? RightDurationDate { get; set; }
    public ICollection<DividendPaymentScheduleResponse>? DividendPaymentSchedules { get; set; }
}
