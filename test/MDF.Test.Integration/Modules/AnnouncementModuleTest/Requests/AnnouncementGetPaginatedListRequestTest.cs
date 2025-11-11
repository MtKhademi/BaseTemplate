namespace MDF.Test.Integration.Modules.AnnouncementModuleTest.Requests;

public class AnnouncementGetPaginatedListRequestTest
{
    public string? EndDateTime { get; set; }
    public string? StartDateTime { get; set; }
    public string? TypeOfAnnouncement { get; set; }
    public bool? IsConfirm { get; set; }
    public int? CodalCode { get; set; } = null;
    public int? AnnouncementId { get; set; } = null;
    public string? PublishDateTime { get; set; } = null;
    public string? SymbolIsin { get; set; }
    public int? FirmId { get; set; }

    public int? CurrentPage { get; set; }
    public int? SizeOfPage { get; set; }
}





