using MDF.Test.Integration.SUTS.APIS.V2.Dtos.SymbolDtos.AdjustedPriceDtos;

namespace MDF.Test.Integration.Modules.SymbolModuleTest.BoundTest.Dtos.SymbolDtoTestBuilders
{
    internal class AdjustedPriceFilterDtoBuilder
    {

        private readonly AdjustedPriceTableFilterDto _dto = new AdjustedPriceTableFilterDto();
        public AdjustedPriceFilterDtoBuilder()
        {
        }
        public AdjustedPriceFilterDtoBuilder WithIsDeleted(bool? value)
        {
            _dto.IsDeleted = value;
            return this;
        }
        public AdjustedPriceFilterDtoBuilder WithDateTime(string? value)
        {
            _dto.DateTime = value;
            return this;
        }

        public AdjustedPriceFilterDtoBuilder WithIsins(List<string>? value)
        {
            _dto.Isins = value;
            return this;
        }


        public AdjustedPriceTableFilterDto Build() => _dto;

    }
}
