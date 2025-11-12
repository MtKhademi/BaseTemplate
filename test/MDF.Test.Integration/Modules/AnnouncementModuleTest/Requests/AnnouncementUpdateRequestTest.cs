namespace MDF.Test.Integration.Modules.AnnouncementModuleTest.Requests;

public record AnnouncementUpdateRequestTest(
 string? SymbolIsin = default!,
 string? Title = default!,
 int? CodalCode = default!,
 string? PublishDateTime = default!
);

