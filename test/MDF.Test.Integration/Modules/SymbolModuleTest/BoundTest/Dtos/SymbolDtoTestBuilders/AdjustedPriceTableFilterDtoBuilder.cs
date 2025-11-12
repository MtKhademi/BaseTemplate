using MDF.Test.Integration.SUTS.APIS.V2.Dtos.SymbolDtos.AdjustedPriceDtos;

namespace MDF.Test.Integration.Modules.SymbolModuleTest.BoundTest.Dtos.SymbolDtoTestBuilders
{
    internal class AdjustedPriceTableFilterDtoV2Builder
    {

        private readonly AdjustedPriceTableFilterDtoV2Test _dto = new AdjustedPriceTableFilterDtoV2Test();
        public AdjustedPriceTableFilterDtoV2Builder()
        {
        }
        public AdjustedPriceTableFilterDtoV2Builder WithIsDeleted(bool? value)
        {
            _dto.IsDeleted = value;
            return this;
        }

        public AdjustedPriceTableFilterDtoV2Builder WithEndPublishDateTime(string? value)
        {
            _dto.EndPublishDateTime = value;
            return this;
        }
        public AdjustedPriceTableFilterDtoV2Builder WithStartPublishDateTime(string? value)
        {
            _dto.StartPublishDateTime = value;
            return this;
        }


        public AdjustedPriceTableFilterDtoV2Builder WithStartDateTime(string? value)
        {
            _dto.StartDateTime = value;
            return this;
        }

        public AdjustedPriceTableFilterDtoV2Builder WithEndDateTime(string? value)
        {
            _dto.EndDateTime = value;
            return this;
        }

        public AdjustedPriceTableFilterDtoV2Builder WithIsins(string? value)
        {
            _dto.Isins = value;
            return this;
        }
        public AdjustedPriceTableFilterDtoV2Builder WithCodalCode(string? value)
        {
            _dto.CodalCode = value;
            return this;
        }


        public AdjustedPriceTableFilterDtoV2Test Build() => _dto;

    }
}
