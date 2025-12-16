namespace Test.Integration.IAMModuleTest.Responses;

public record ModuleResponseTest(
    int? ModuleId = default!,
    string? ModuleName = default!,
    string? ModuleDescription = default!);