namespace Infrastructure.Extentions;

public static class ExceptionExtentions
{
    public static List<string> GetErrors(this Exception ex)
    {
        var oResult = new List<string>();
        if (ex != null)
        {
            oResult.Add(ex.Message);
            if (ex.InnerException != null)
                oResult.AddRange(GetErrors(ex.InnerException));
        }

        return oResult;
    }

}
