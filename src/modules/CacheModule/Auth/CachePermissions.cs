namespace CacheModule.Auth;

internal class CachePermissions
{
    internal static AppModule CacheModule = new AppModule("Cache", "Cache Module");

    internal static AppFeature CacheFeature = new AppFeature("Cache", "Cache Management Feature");


    //----------- cache permissions
    internal static ApiPermission Delete => new ApiPermission(CacheModule, CacheFeature, ActionType.Delete);
    internal static ApiPermission Read => new ApiPermission(CacheModule, CacheFeature, ActionType.Read);
    internal static ApiPermission Create => new ApiPermission(CacheModule, CacheFeature, ActionType.Create);

}