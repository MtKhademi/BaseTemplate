using Infrastructure.Module;

namespace ECommerceModule.Auth;

internal class ECommercePermissions
{
    internal static AppModule ECommerceModule = new AppModule("ECommerce", "ECommerce Module");

    //----------- features
    internal static AppFeature OrderFeature = new AppFeature("ECommerce", "ECommerce Management order feature");
    internal static AppFeature CatalogFeature = new AppFeature("ECommerce", "ECommerce Management catalog feature");

    //----------- cache permissions
    internal static ApiPermission Delete => new ApiPermission(ECommerceModule, OrderFeature, ActionType.Delete);
    internal static ApiPermission Read => new ApiPermission(ECommerceModule, OrderFeature, ActionType.Read);
    internal static ApiPermission Create => new ApiPermission(ECommerceModule, OrderFeature, ActionType.Create);
    internal static ApiPermission Update => new ApiPermission(ECommerceModule, OrderFeature, ActionType.Update);

    //----------- catalog permissions
    internal static ApiPermission CatalogDelete => new ApiPermission(ECommerceModule, CatalogFeature, ActionType.Delete);
    internal static ApiPermission CatalogRead => new ApiPermission(ECommerceModule, CatalogFeature, ActionType.Read);
    internal static ApiPermission CatalogCreate => new ApiPermission(ECommerceModule, CatalogFeature, ActionType.Create);
    internal static ApiPermission CatalogUpdate => new ApiPermission(ECommerceModule, CatalogFeature, ActionType.Update);

}