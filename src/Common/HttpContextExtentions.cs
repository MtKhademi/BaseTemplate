using Microsoft.AspNetCore.Http;

namespace MDF.Common;

public static class HttpContextExtentions
{
    public static string GetUserFromContext(this HttpContext context)
    {
        var curentUser = context.User.Identity.Name;
        return string.IsNullOrWhiteSpace(curentUser) ? "Anonymous" : curentUser;
    }
}
