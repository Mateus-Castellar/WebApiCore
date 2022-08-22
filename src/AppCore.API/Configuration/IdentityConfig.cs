using AppCore.API.Data;
using AppCore.API.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AppCore.API.Configuration
{
    public static class IdentityConfig
    {
        public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services, IConfiguration configuration,
            string connectionString)
        {
            services.AddDbContext<ApiIdentityDbContext>(options => options
                .UseSqlServer(connectionString));

            services.AddDefaultIdentity<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApiIdentityDbContext>()
                .AddErrorDescriber<IdentityTraducaoMensagensErro>()
                .AddDefaultTokenProviders();

            //Json Web Token
            var appSettingsSection = configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);//obtem a chave do appSettings..

            services.AddAuthentication(lbda =>
            {
                //config app para funcionar baseada em token
                lbda.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                lbda.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(lbda =>
            {
                lbda.RequireHttpsMetadata = true; //requer https
                lbda.SaveToken = true;
                lbda.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = appSettings.ValidoEm,
                    ValidIssuer = appSettings.Emissor
                };
            });

            return services;
        }
    }
}