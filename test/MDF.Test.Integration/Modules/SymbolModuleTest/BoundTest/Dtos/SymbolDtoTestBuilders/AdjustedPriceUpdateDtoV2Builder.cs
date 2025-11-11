using MDF.Test.Integration.SUTS.APIS.V2.Dtos.SymbolDtos.AdjustedPriceDtos;

namespace MDF.Test.Integration.Modules.SymbolModuleTest.BoundTest.Dtos.SymbolDtoTestBuilders
{
    internal class AdjustedPriceUpdateDtoV2Builder
    {

        private readonly AdjustedPriceUpdateDtoV2Test _dto = new AdjustedPriceUpdateDtoV2Test();
        public AdjustedPriceUpdateDtoV2Builder()
        {
        }
        public AdjustedPriceUpdateDtoV2Builder WithAdjustedPriceId(int? value)
        {
            _dto.AdjustedPriceId = value;
            return this;
        }
        public AdjustedPriceUpdateDtoV2Builder WithCodalCode(int? value)
        {
            _dto.CodalCode = value;
            return this;
        }

        public AdjustedPriceUpdateDtoV2Builder WithAdjustedPrice(decimal? value)
        {
            _dto.AdjustedPrice = value;
            return this;
        }

        public AdjustedPriceUpdateDtoV2Builder WithAdjustedLastPrice(decimal? value)
        {
            _dto.AdjustedLastPrice = value;
            return this;
        }

        public AdjustedPriceUpdateDtoV2Builder WithIsAdjusted(bool? value)
        {
            _dto.IsAdjusted = value;
            return this;
        }


        public AdjustedPriceUpdateDtoV2Test Build() => _dto;

    }
}
