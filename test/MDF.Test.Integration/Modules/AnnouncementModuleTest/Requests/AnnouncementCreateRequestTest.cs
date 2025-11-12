namespace MDF.Test.Integration.Modules.AnnouncementModuleTest.Requests;

public record AnnouncementCreateRequestTest(
 string? SymbolIsin = default!,
 string? Title = default!,
 int? CodalCode = default!,
 string? PublishDateTime = default!
);

