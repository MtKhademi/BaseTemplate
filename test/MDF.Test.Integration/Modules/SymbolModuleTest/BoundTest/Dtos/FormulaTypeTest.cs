using System.Text.Json.Serialization;

namespace MDF.Test.Integration.Modules.SymbolModuleTest.BoundTest.Dtos;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum FormulaTypeTest
{
    NotSet = 0,
    /// <summary>
    /// اول
    /// </summary>
    One = 1,
    /// <summary>
    /// دوم
    /// </summary>
    Two = 2,
    /// <summary>
    /// سوم
    /// </summary>
    Three = 3,
    /// <summary>
    /// چهارم
    /// </summary>
    Four = 4,
    /// <summary>
    /// اکسل
    /// </summary>
    Excel = 100,
    /// <summary>
    /// سلف
    /// </summary>
    Salaf = 200
}
