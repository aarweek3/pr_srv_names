using AutoMapper;
using FluentValidation;
using pr_srv_names.Deepl;
using pr_srv_names.Extensions;
using pr_srv_names.Middleware;
using pr_srv_names.Supports.Deepl;


var builder = WebApplication.CreateBuilder(args);

// =================================================================
// 1. КОНФИГУРАЦИЯ БИЛДЕРА (WebHost, Logging)
// =================================================================
builder.WebHost.ConfigureKestrel(serverOptions => { serverOptions.AddServerHeader = false; });

builder.ConfigureLogging();

// =================================================================
// 2. РЕГИСТРАЦИЯ СЕРВИСОВ (ОБЯЗАТЕЛЬНО ДО builder.Build())
// =================================================================

// Конфигурация базы данных
builder.Services.AddDatabaseConfiguration(builder.Configuration);

// Конфигурация Identity
builder.Services.AddIdentityConfiguration(builder.Configuration);
builder.Services.AddIdentityPolicies();

// Конфигурация JWT
builder.Services.AddJwtAuthentication(builder.Configuration, builder.Environment);
builder.Services.AddJwtSettings(builder.Configuration);
builder.Services.ConfigureJwtCookies(builder.Configuration);

// Конфигурация CORS
builder.Services.AddCorsConfiguration(builder.Configuration, builder.Environment);

// Регистрация общих сервисов и Swagger
builder.Services.RegisterAllServices();
builder.Services.AddSwaggerDocumentation();

// AutoMapper
var mapperConfig = new MapperConfiguration(cfg => 
    cfg.AddMaps(typeof(Program).Assembly));
builder.Services.AddSingleton(mapperConfig.CreateMapper());

// FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// HttpClient для DeepL
builder.Services.AddHttpClient<IDeepLTranslationService, DeepLTranslationService>(client =>
{
    client.Timeout = TimeSpan.FromSeconds(30);
});

// OAuth Providers (Google, Facebook)
builder.Services.AddOAuthProviders(builder.Configuration);

// Domain Services и Repositories
builder.Services.AddDomainServices();

// HealthChecks
builder.Services.AddApplicationHealthChecks();

var app = builder.Build();

// =================================================================
// 3. КОНФИГУРАЦИЯ PIPELINE ПРИЛОЖЕНИЯ (ПОСЛЕ app.Build())
// =================================================================

// Инициализация базы данных
await app.InitializeDatabaseAsync();

// Конфигурация pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerDocumentation();
}

// 1. Обработчик ошибок
app.UseMiddleware<AuthExceptionMiddleware>();

// 2. CORS (должен быть до маршрутизации)
app.UseCorsConfiguration(app.Environment);

// 3. HTTPS редирект
app.UseHttpsRedirection();

// 4. Заголовки безопасности
app.UseHsts();
app.UseMiddleware<SecurityHeadersMiddleware>();

// 5. Статические файлы
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        ctx.Context.Response.Headers.Append("Access-Control-Allow-Origin", "*");
        ctx.Context.Response.Headers.Append("Access-Control-Allow-Methods", "GET, OPTIONS");
        ctx.Context.Response.Headers.Append("Access-Control-Allow-Headers", "Content-Type");
    }
});

// 6. Routing
app.UseRouting();

// 7. Модификация cookies
app.AddCookieModificationMiddleware(app.Environment);

// 8. Аутентификация и авторизация
app.UseAuthentication();
app.UseAuthorization();

// 9. Rate limiting
app.UseMiddleware<RateLimitingMiddleware>();

// 10. Debug endpoints (только в development)
if (app.Environment.IsDevelopment())
{
    app.ConfigureDebugEndpoints();
}

// 11. Контроллеры
app.MapControllers();

app.Run();