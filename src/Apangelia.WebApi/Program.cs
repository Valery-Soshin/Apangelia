using Apangelia.WebApi.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.ConfigureServices(builder.Configuration);

var app = builder.Build();

app.ApplyMigrations();

app.UseSwaggerConf();

app.UseHttpLogging();

app.MapDefaultEndpoints();
app.MapEndpoints();

app.Run();
