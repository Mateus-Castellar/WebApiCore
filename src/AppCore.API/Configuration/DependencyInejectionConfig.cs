using AppCore.Business.Interfaces;
using AppCore.Data.Context;
using AppCore.Data.Repository;

namespace AppCore.API.Configuration
{
    public static class DependencyInejectionConfig
    {
        public static IServiceCollection ResolveDependences(this IServiceCollection services)
        {
            services.AddScoped<AppCoreDbContext>();
            services.AddScoped<IFornecedorRepository, FornecedorRepository>();

            return services;
        }
    }
}
