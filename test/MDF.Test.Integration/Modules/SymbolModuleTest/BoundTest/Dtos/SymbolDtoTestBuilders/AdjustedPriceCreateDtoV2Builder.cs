using MDF.Test.Integration.SUTS.APIS.V2.Dtos.SymbolDtos.AdjustedPriceDtos;

namespace MDF.Test.Integration.Modules.SymbolModuleTest.BoundTest.Dtos.SymbolDtoTestBuilders
{
    internal class AdjustedPriceCreateDtoV2Builder
    {
        private readonly AdjustedPriceCreateDtoV2Test _dto = new AdjustedPriceCreateDtoV2Test();
        public AdjustedPriceCreateDtoV2Builder()
        {
        }

        public AdjustedPriceCreateDtoV2Builder WithIsin(string? value)
        {
            _dto.Isin = value;
            return this;
        }
        public AdjustedPriceCreateDtoV2Builder WithFirmId(int? value)
        {
            _dto.FirmId = value;
            return this;
        }
        public AdjustedPriceCreateDtoV2Builder WithEndDateTime(string? value)
        {
            _dto.EndDateTime = value;
            return this;
        }
        public AdjustedPriceCreateDtoV2Builder WithStartDateTime(string? value)
        {
            _dto.StartDateTime = value;
            return this;
        }


        public AdjustedPriceCreateDtoV2Test Build() => _dto;
    }
}
