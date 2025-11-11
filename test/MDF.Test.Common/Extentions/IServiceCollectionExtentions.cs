using MDF.Modules.Common.Providers.DateTimeProviders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace MDF.Test.Common.Extentions;

public static class IServiceCollectionExtentions
{
    public static void SetIDateTimeProvider_Now_Moq(this IServiceCollection services, DateTime dtForSetOnNowProperty)
    {
        var iDateTimeProvider = new Mock<IDateTimeProvider>();
        iDateTimeProvider.Setup(d => d.Now)
            .Returns(dtForSetOnNowProperty);

        services.RemoveAll<IDateTimeProvider>();
        services.AddSingleton<IDateTimeProvider>(iDateTimeProvider.Object);
    }
}
