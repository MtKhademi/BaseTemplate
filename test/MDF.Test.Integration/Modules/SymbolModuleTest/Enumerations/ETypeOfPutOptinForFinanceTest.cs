using System.ComponentModel;

namespace MDF.Test.Integration.Modules.SymbolModuleTest.Enumerations;

public enum ETypeOfPutOptinForFinanceTest : byte
{
    [Description("تایین نشده")]
    NotSet = 0,
    [Description("با هدف غیر از تامین مالی")]
    OtherFinancing = 1,
    [Description("با هدف تامین مالی")]
    Financing = 2
}
