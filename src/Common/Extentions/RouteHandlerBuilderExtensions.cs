using Infrastructure.Module;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Extentions;

public static class RouteHandlerBuilderExtensions
{
    public static RouteHandlerBuilder WithPermission(this RouteHandlerBuilder builder, ApiPermission permission)
    {
        builder.RequireAuthorization();
        builder.WithMetadata(permission.ToApiPermission());
        return builder;
    }
}
