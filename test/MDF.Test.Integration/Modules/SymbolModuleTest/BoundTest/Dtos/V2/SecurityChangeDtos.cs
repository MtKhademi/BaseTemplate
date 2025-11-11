namespace MDF.Test.Integration.SUTS.APIS.V2.Dtos.SymbolDtos
{
    #region SecurityExchangeAddDto

    public class SecurityExchangeCreateOrUpdateDto
    {
        public byte SecuritiesExchangeIdPk { get; set; }
        public string Title { get; set; } = null!;
        public byte Code { get; set; }

    }




    #endregion

    #region SecurityExchangeDeleteDto
    public class SecurityExchangeDeleteDTO
    {
        public int? SecurityExchangeId { get; set; }
    }
    #endregion

    #region SecurityExchangeGetDto
    public class SecurityExchangeGetDto
    {
        public string Title { get; set; } = null!;
        public byte Code { get; set; }
    }
    #endregion

}
