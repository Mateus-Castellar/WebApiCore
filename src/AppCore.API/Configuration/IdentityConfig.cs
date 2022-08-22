using AppCore.API.Data;
using Microsoft.EntityFrameworkCore;

namespace AppCore.API.Configuration
{
    public static class IdentityConfig
    {
        public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services,
            string connectionString)
        {
            services.AddDbContext<ApiIdentityDbContext>(options => options
                .UseSqlServer(connectionString));

            return services;
        }
    }
}
