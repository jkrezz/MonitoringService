using Microsoft.OpenApi.Models;
using WebApi.Data;
using WebApi.Middleware;
using WebApi.Repositories;
using WebApi.Repositories.Interfaces;
using WebApi.Services;
using WebApi.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    options.SwaggerDoc("v1", new OpenApiInfo {
        Title = "Device Monitoring API",
        Version = "v1"
    });
});

builder.Services.AddSingleton<DapperContext>();
builder.Services.AddSingleton<DbInitializer>();
builder.Services.AddScoped<IDeviceRepository, DeviceRepository>();
builder.Services.AddScoped<ISessionRepository, SessionRepository>();
builder.Services.AddScoped<ISessionService, SessionService>();
builder.Services.AddTransient<ExceptionHandlingMiddleware>();

var app = builder.Build();

// Инициализация базы данных при старте приложения.
await using (var scope = app.Services.CreateAsyncScope())
{
    var initializer = scope.ServiceProvider.GetRequiredService<DbInitializer>();
    await initializer.InitializeAsync();
}

// Глобальная обработка исключений.
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();