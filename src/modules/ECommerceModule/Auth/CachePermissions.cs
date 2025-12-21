using Infrastructure.Module;

namespace ECommerceModule.Auth;

internal class ECommercePermissions
{
    internal static AppModule ECommerceModule = new AppModule("ECommerce", "ECommerce Module");

    //----------- features
    internal static AppFeature OrderFeature = new AppFeature("ECommerce", "ECommerce Management order feature");

    //----------- cache permissions
    internal static ApiPermission Delete => new ApiPermission(ECommerceModule, OrderFeature, ActionType.Delete);
    internal static ApiPermission Read => new ApiPermission(ECommerceModule, OrderFeature, ActionType.Read);
    internal static ApiPermission Create => new ApiPermission(ECommerceModule, OrderFeature, ActionType.Create);

}