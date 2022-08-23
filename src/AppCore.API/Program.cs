using AppCore.API.Configuration;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.AddEnviromentConfiguration();

builder.Services.AddIdentityConfiguration(builder.Configuration, connectionString);

builder.Services.AddApiConfiguration(connectionString);

builder.Services.AddSwaggerConfig();

builder.Services.ResolveDependences();


var app = builder.Build();
var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

app.UseSwaggerConfig(apiVersionDescriptionProvider);

app.UseAuthentication();

app.UseApiConfiguration();

app.Run();
