using MDF.Test.Integration.SUTS.APIS.V2.Dtos.SymbolDtos;

namespace MDF.Test.Integration.Modules.SymbolModuleTest.BoundTest.Dtos.SymbolDtoTestBuilders
{
    internal class SymbolFilterDtoBuilder
    {

        private readonly SymbolFilterDto _dto = new SymbolFilterDto();

        public SymbolFilterDtoBuilder()
        { }
        public SymbolFilterDtoBuilder WithTypeOfSymbols(List<string> value)
        {
            _dto.TypeOfSymbols = value;
            return this;
        }
        public SymbolFilterDtoBuilder WithFirmId(int? value)
        {
            _dto.FirmId = value;
            return this;
        }
        public SymbolFilterDtoBuilder WithIsins(List<string>? value)
        {
            _dto.Isins = value;
            return this;
        }

        public SymbolFilterDto Build() => _dto;

    }
}
