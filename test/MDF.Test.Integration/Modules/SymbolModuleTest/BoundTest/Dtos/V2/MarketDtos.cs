namespace MDF.Test.Integration.SUTS.APIS.V2.Dtos.SymbolDtos
{
    #region MarketAddDto

    public class MarketCreateOrUpdateDto
    {
        public int MarketIdPk { get; set; }
        public string? Title { get; set; }
        public string? EnTitle { get; set; }
        public string? Code { get; set; }

    }




    #endregion

    #region MarketDeleteDto
    public class MarketDeleteDTO
    {
        public int? MarketId { get; set; }
    }
    #endregion

    #region MarketGetDto
    public class MarketGetDto
    {
        public string? Title { get; set; }
        public string? EnTitle { get; set; }
        public string? Code { get; set; }

        public override string ToString()
            => $"{Title} => {Code}";
    }
    #endregion



}
