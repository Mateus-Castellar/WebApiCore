using AppCore.API.Configuration;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.AddEnviromentConfiguration();

builder.Services.AddIdentityConfiguration(connectionString);

builder.Services.AddApiConfiguration(connectionString);

builder.Services.ResolveDependences();


var app = builder.Build();

app.UseApiConfiguration();

app.Run();
