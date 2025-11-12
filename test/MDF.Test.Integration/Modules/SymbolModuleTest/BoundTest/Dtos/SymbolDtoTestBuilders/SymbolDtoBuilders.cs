using Test.Integration.ModulesTest.SymbolModuleTest.Enumerations;

namespace MDF.Test.Integration.Modules.SymbolModuleTest.BoundTest.Dtos.SymbolDtoTestBuilders
{
    internal class SymbolUpdateDtoTestBuilder
    {

        private readonly SymbolUpdateDtoTest _dto = new SymbolUpdateDtoTest();
        public SymbolUpdateDtoTestBuilder()
        {
            _dto.Isin = "";
        }

        public SymbolUpdateDtoTestBuilder WithIsin(string value)
        {
            _dto.Isin = value;
            return this;
        }

        public SymbolUpdateDtoTestBuilder WithEnSymbol(string value)
        {
            _dto.EnSymbol = value;
            return this;
        }
        public SymbolUpdateDtoTestBuilder WithSymbolName(string value)
        {
            _dto.SymbolName = value;
            return this;
        }

        public SymbolUpdateDtoTestBuilder WithSecurityExchangeCodeForExchangeMarket(byte? value)
        {
            _dto.SecurityExchangeCodeForExchangeMarket = value;
            return this;
        }

        public SymbolUpdateDtoTestBuilder WithBoardCode(byte? value)
        {
            _dto.BoardCode = value;
            return this;
        }
        public SymbolUpdateDtoTestBuilder WithMarketCode(string? value)
        {
            _dto.MarketCode = value;
            return this;
        }
        public SymbolUpdateDtoTestBuilder WithSecurityExchangeCodeForExchangeBoard(byte? value)
        {
            _dto.SecurityExchangeCodeForExchangeBoard = value;
            return this;
        }
        public SymbolUpdateDtoTestBuilder WithInstrumentId(int value)
        {
            _dto.InstrumentId = value;
            return this;
        }
        public SymbolUpdateDtoTestBuilder WithSymbolGroupId(byte value)
        {
            _dto.SymbolGroupId = value;
            return this;
        }
        public SymbolUpdateDtoTestBuilder WithFirmId(int? value)
        {
            _dto.FirmId = value;
            return this;
        }
        public SymbolUpdateDtoTestBuilder WithIsDisabled(bool? value)
        {
            _dto.IsDisabled = value;
            return this;
        }
        public SymbolUpdateDtoTestBuilder WithTypeOfSymbol(TypeOfSymbolTest? value)
        {
            _dto.TypeOfSymbol = value;
            return this;
        }

        public SymbolUpdateDtoTest Build() => _dto;

    }
}
