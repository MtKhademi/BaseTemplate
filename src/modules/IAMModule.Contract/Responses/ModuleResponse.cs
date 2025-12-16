namespace IAMModule.Contract.Responses;

public record ModuleResponse(
    int? ModuleId = default!,
    string? ModuleName = default!,
    string? ModuleDescription = default!);