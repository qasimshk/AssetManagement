using assetmgmt.core.Abstractions.Mappers;
using assetmgmt.core.Abstractions.Services;
using assetmgmt.core.Services;
using assetmgmt.core.Mappers;

namespace assetmgmt.api.Extensions
{
    public static class ApplicationDependencies
    {
        public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
        {
            // Services
            services.AddScoped<IAssetServices, AssetServices>();

            // Mappers
            services.AddScoped<IAssetMapper, AssetMapper>();

            return services;
        }
    }
}
