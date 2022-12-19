using AppCore.API.Extensions;
using AppCore.Business.Interfaces;
using AppCore.Business.Notificacoes;
using AppCore.Business.Services;
using AppCore.Data.Context;
using AppCore.Data.Repository;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AppCore.API.Configuration;

public static class DependencyInejectionConfig
{
    public static IServiceCollection ResolveDependences(this IServiceCollection services)
    {
        services.AddScoped<AppCoreDbContext>();
        services.AddScoped<IFornecedorRepository, FornecedorRepository>();
        services.AddScoped<IEnderecoRepository, EnderecoRepository>();
        services.AddScoped<IProdutoRepository, ProdutoRepository>();

        services.AddScoped<IFornecedorService, FornecedorService>();
        services.AddScoped<IProdutoService, ProdutoService>();
        services.AddScoped<INotificador, Notificador>();

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<IUser, AspNetUser>();

        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

        return services;
    }
}