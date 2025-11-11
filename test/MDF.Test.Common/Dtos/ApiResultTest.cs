namespace MDF.Test.Common.Dtos;

public enum ETypeOfApiResultStatusCodeTest
{
    Default = 0,
    Success = 1,
    NotFound = 2,
    UnAuthentication = 3,
    UnAuthorization = 4,
    BadRequest = 5,
    InternalServerError = 6,
    AlreadyExist = 7,
}

public class ApiResultTest
{
    public bool IsSuccess { get; set; }
    public ETypeOfApiResultStatusCodeTest StatusCode { get; set; } = ETypeOfApiResultStatusCodeTest.Default;
    public IEnumerable<string> Messages { get; set; } = new List<string>();
    public string? ErrorKey { get; set; }
    public ApiResultTest()
    {
    }

    public override string ToString()
    {
        return
            $"{IsSuccess} - " +
            $"{StatusCode} - " +
            $"[{string.Join(" | ", Messages)}]";
    }


    public virtual ApiResultTest Success()
    {
        IsSuccess = true;
        Messages = new List<string>() { "SUCCESS" };
        StatusCode = ETypeOfApiResultStatusCodeTest.Success;
        return this;
    }
    public virtual ApiResultTest BadRequest(params string[] messages) => BadRequest(messages.ToList());
    public virtual ApiResultTest BadRequest(IEnumerable<string> messages)
    {
        IsSuccess = false;
        Messages = messages;
        StatusCode = ETypeOfApiResultStatusCodeTest.BadRequest;
        return this;
    }
    public virtual ApiResultTest InternalServerError()
    {
        StatusCode = ETypeOfApiResultStatusCodeTest.InternalServerError;
        IsSuccess = false;
        Messages = new List<string>() { "خطایی پیشبینی نشده رخ داده است." };
        return this;
    }

}
public class ApiResultTest<TData> : ApiResultTest
{
    public TData Result { get; set; } = default;

    public ApiResultTest() { }

    public override string ToString()
    {
        return base.ToString() + $" RESULT : {Result}";
    }


    public ApiResultTest<TData> Success(TData data)
    {
        IsSuccess = true;
        Messages = new List<string>() { "SUCCESS" };
        StatusCode = ETypeOfApiResultStatusCodeTest.Success;
        Result = data;
        return this;
    }
    public override ApiResultTest<TData> BadRequest(params string[] messages) => BadRequest(messages.ToList());
    public override ApiResultTest<TData> BadRequest(IEnumerable<string> messages)
    {
        IsSuccess = false;
        Messages = messages;
        StatusCode = ETypeOfApiResultStatusCodeTest.BadRequest;
        Result = default;
        return this;
    }
    public override ApiResultTest<TData> InternalServerError()
    {
        StatusCode = ETypeOfApiResultStatusCodeTest.InternalServerError;
        IsSuccess = false;
        Messages = new List<string>() { "خطایی پیشبینی نشده رخ داده است." };
        Result = default;
        return this;
    }

}
