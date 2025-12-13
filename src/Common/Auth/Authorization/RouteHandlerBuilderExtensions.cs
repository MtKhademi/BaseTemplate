using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Auth.Authorization;

public static class RouteHandlerBuilderExtensions
{
    public static RouteHandlerBuilder WithPermission(this RouteHandlerBuilder builder, AppPermission permission)
    {
        builder.RequireAuthorization();
        builder.WithMetadata(permission.ToApiPermission());
        return builder;
    }
}
