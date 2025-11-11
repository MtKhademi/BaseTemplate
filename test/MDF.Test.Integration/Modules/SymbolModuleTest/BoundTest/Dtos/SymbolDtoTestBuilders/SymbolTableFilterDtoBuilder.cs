using MDF.Test.Integration.SUTS.APIS.V2.Dtos.SymbolDtos;

namespace MDF.Test.Integration.Modules.SymbolModuleTest.BoundTest.Dtos.SymbolDtoTestBuilders
{
    internal class SymbolTableFilterDtoBuilder
    {

        private readonly SymbolTableFilterDto _dto = new SymbolTableFilterDto();
        public SymbolTableFilterDtoBuilder()
        { }
        public SymbolTableFilterDtoBuilder WithIsDisable(bool? value)
        {
            _dto.IsDisable = value;
            return this;
        }
        public SymbolTableFilterDtoBuilder WithMarketCode(string? value)
        {
            _dto.MarketCode = value;
            return this;
        }
        public SymbolTableFilterDtoBuilder WithFirmId(int? value)
        {
            _dto.FirmId = value;
            return this;
        }
        public SymbolTableFilterDtoBuilder WithSymbolName(string? value)
        {
            _dto.SymbolNameOrIsin = value;
            return this;
        }
        public SymbolTableFilterDtoBuilder WithIsins(List<string>? value)
        {
            _dto.Isins = value;
            return this;
        }
        public SymbolTableFilterDtoBuilder WithHasFirm(bool? value)
        {
            _dto.HasFirm = value;
            return this;
        }

        public SymbolTableFilterDto Build() => _dto;

    }
}
