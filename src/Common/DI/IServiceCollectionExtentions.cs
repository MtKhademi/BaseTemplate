namespace Common.DI;

public static class IServiceCollectionExtentions
{
    public static IServiceCollection RegistersServices<TType>(this IServiceCollection services,Assembly assembly)
    {
        Type baseType = typeof(TType);

        IEnumerable<string> baseTypes = baseType.Assembly.DefinedTypes.Where(x => x.IsInterface && x.Namespace == baseType.Namespace)
            .Select(x => x.Name.Trim().ToLower())
            .ToList();

        var implementationsType = assembly.GetTypes()
        .Where(x => !x.IsInterface && x.GetInterface(baseType.Name) != null);

        foreach (var implementationType in implementationsType)
        {
            var servicesType = implementationType.GetInterfaces()
                .Where(r => !baseTypes.Contains(r.Name.Trim().ToLower()));

            foreach (var serviceType in servicesType)
            {

                var dontAutomaticDependencyInjectionAttribute = implementationType.GetCustomAttribute<DIDontInjectAutomatic>();
                if (dontAutomaticDependencyInjectionAttribute != null)
                    continue;

                var scopeAttribute = implementationType.GetCustomAttribute<DIScopeAttribute>();
                if (scopeAttribute == null)
                    scopeAttribute = new DIScopeAttribute();
                switch (scopeAttribute.ScopeType)
                {
                    case DIScopeType.Transiant:
                        services.AddTransient(serviceType, implementationType);
                        break;
                    case DIScopeType.Singleton:
                        services.AddSingleton(serviceType, implementationType);
                        break;
                    case DIScopeType.Scope:
                    default:
                        services.AddScoped(serviceType, implementationType);
                        break;
                }
            }
        }

        return services;
    }
}