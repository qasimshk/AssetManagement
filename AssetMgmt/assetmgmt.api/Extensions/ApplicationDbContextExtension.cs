using assetmgmt.data.Context;
using Microsoft.EntityFrameworkCore;

namespace assetmgmt.api.Extensions
{
    public static class ApplicationDbContextExtension
    {
        public static IServiceCollection AddApplicationDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AssetDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), sqlOptions =>
                sqlOptions.EnableRetryOnFailure(10, TimeSpan.FromSeconds(30), null)));

            return services;
        }
    }
}
