using MDF.DAL.Modules.Entities.OldEntities;
using MDF.Modules.SymbolModule.Depricate.Abstractions;

namespace MDF.Test.Integration.Modules.SymbolModuleTest.BoundTest.Dtos.SymbolDtoTestBuilders;

internal class SymbolEntityBuilderTest
{

    private readonly Symbol _entity = new Symbol();
    public SymbolEntityBuilderTest()
    {
    }


    public SymbolEntityBuilderTest WithTypeOfSymbolInTseTmc(TypeOfSymbolTest value)
    {
        _entity.TypeOfSymbolInTseTmc = (ETypeOfSymbol)value;
        return this;
    }
    public SymbolEntityBuilderTest WithTypeOfSymbol(TypeOfSymbolTest value)
    {
        _entity.TypeOfSymbol = (ETypeOfSymbol)value;
        return this;
    }
    public SymbolEntityBuilderTest WithExchangeBoardIdFk(byte value)
    {
        _entity.ExchangeBoardIdFk = value;
        return this;
    }
    public SymbolEntityBuilderTest WithSymbolGroupIdFk(byte value)
    {
        _entity.SymbolGroupIdFk = value;
        return this;
    }
    public SymbolEntityBuilderTest WithExchangeTypeIdFk(byte? value)
    {
        _entity.ExchangeTypeIdFk = value;
        return this;
    }
    public SymbolEntityBuilderTest WithMaxQuantityOrder(int value)
    {
        _entity.MaxQuantityOrder = value;
        return this;
    }
    public SymbolEntityBuilderTest WithMinQuantityOrder(int value)
    {
        _entity.MinQuantityOrder = value;
        return this;
    }
    public SymbolEntityBuilderTest WithExchangeMarketIdFk(int value)
    {
        _entity.ExchangeMarketIdFk = value;
        return this;
    }
    public SymbolEntityBuilderTest WithInstrumentIdFk(int value)
    {
        _entity.InstrumentIdFk = value;
        return this;
    }
    public SymbolEntityBuilderTest WithLot(int value)
    {
        _entity.Lot = value;
        return this;
    }
    public SymbolEntityBuilderTest WithIsDisabled(bool? value)
    {
        _entity.IsDisabled = value;
        return this;
    }
    public SymbolEntityBuilderTest WithIsImport(bool? value)
    {
        _entity.IsImport = value;
        return this;
    }
    public SymbolEntityBuilderTest WithIsCompelete(bool? value)
    {
        _entity.IsCompelete = value;
        return this;
    }
    public SymbolEntityBuilderTest WithIsImported(bool? value)
    {
        _entity.IsImported = value;
        return this;
    }
    public SymbolEntityBuilderTest WithInstCodeTse(long? value)
    {
        _entity.InstCodeTse = value;
        return this;
    }
    public SymbolEntityBuilderTest WithSymbolCodeTseSafeEncoding(string value)
    {
        _entity.SymbolCodeTseSafeEncoding = value;
        return this;
    }
    public SymbolEntityBuilderTest WithBourseCode(string value)
    {
        _entity.BourseCode = value;
        return this;
    }
    public SymbolEntityBuilderTest WithSymbolNameModified(string value)
    {
        _entity.SymbolNameModified = value;
        return this;
    }
    public SymbolEntityBuilderTest WithSymbolNameTse(string value)
    {
        _entity.SymbolNameTse = value;
        return this;
    }
    public SymbolEntityBuilderTest WithEnSymbol(string value)
    {
        _entity.EnSymbol = value;
        return this;
    }
    public SymbolEntityBuilderTest WithIsin(string value)
    {
        _entity.Isin = value;
        return this;
    }
    public SymbolEntityBuilderTest WithSymbolName(string value)
    {
        _entity.SymbolName = value;
        return this;
    }
    public SymbolEntityBuilderTest WithTitle(string value)
    {
        _entity.Title = value;
        return this;
    }
    public SymbolEntityBuilderTest WithCdsSymbolName(string value)
    {
        _entity.CdsSymbolName = value;
        return this;
    }
    public SymbolEntityBuilderTest WithFirmId(int? value)
    {
        _entity.FirmId = value;
        return this;
    }
    public SymbolEntityBuilderTest WithBaseVolume(int? value)
    {
        _entity.BaseVolume = value;
        return this;
    }
    public SymbolEntityBuilderTest WithSettlementPeriod(int? value)
    {
        _entity.SettlementPeriod = value;
        return this;
    }
    public SymbolEntityBuilderTest WithDateOfEvent(DateTime? value)
    {
        _entity.DateOfEvent = value;
        return this;
    }
    public SymbolEntityBuilderTest WithEntryDate(DateTime? value)
    {
        _entity.EntryDate = value;
        return this;
    }
    public SymbolEntityBuilderTest WithDisableDateTime(DateTime? value)
    {
        _entity.DisableDateTime = value;
        return this;
    }
    public SymbolEntityBuilderTest WithCreatedDateTime(DateTime? value)
    {
        _entity.CreatedDateTime = value;
        WithLastModifiedDate(value);
        WithEntryDate(value);
        WithDateOfEvent(value);
        return this;
    }
    public SymbolEntityBuilderTest WithLastModifiedDate(DateTime? value)
    {
        _entity.LastModifiedDate = value;
        return this;
    }
    public SymbolEntityBuilderTest WithInstrumentId(int value)
    {
        _entity.InstrumentIdFk = value;
        return this;
    }
    public SymbolEntityBuilderTest WithSymbolGroupId(byte value)
    {
        _entity.SymbolGroupIdFk = value;
        return this;
    }


    public Symbol Build() => _entity;

}
