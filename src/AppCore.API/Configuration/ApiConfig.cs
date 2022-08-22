using AppCore.Data.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace AppCore.API.Configuration
{
    public static class ApiConfig
    {
        public static void AddApiConfiguration(this IServiceCollection services, string connectionString)
        {
            services.Configure<ApiBehaviorOptions>(lbda =>
            {
                lbda.SuppressModelStateInvalidFilter = true;
            });

            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen();

            services.AddControllers().AddJsonOptions(lbda
                => lbda.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

            services.AddDbContext<AppCoreDbContext>(options => options
                .UseSqlServer(connectionString));

            services.AddAutoMapper(typeof(Program));

        }

        public static void UseApiConfiguration(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

        }
    }
}