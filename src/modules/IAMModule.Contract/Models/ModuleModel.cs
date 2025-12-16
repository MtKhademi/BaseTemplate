namespace IAMModule.Contract.Models;

public record ModuleModel(
    int ModuleId,
    string? ModuleName,
    string? ModuleDescription)
{
    public ModuleResponse ToModuleResponse()
        => new ModuleResponse(
            ModuleId: ModuleId,
            ModuleName: ModuleName,
            ModuleDescription: ModuleDescription);
}