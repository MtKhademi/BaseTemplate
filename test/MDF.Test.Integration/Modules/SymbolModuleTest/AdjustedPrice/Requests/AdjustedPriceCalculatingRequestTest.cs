namespace MDF.Test.Integration.Modules.SymbolModuleTest.AdjustedPrice.Requests;

public record AdjustedPriceCalculatingRequestTest(
  string? Isin = null,
  string? Date = null,
  int? FirmId = null);

