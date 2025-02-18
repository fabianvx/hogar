using Serilog;
using Serilog.Events;
using System.Text;
using Hogar.Web.Middleware;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Hogar.Infraestructure.Data;
using Hogar.Infraestructure.Repository.Interfaces;
using Hogar.Infraestructure.Repository.Implementations;
using Hogar.Application.Services.Interfaces;
using Hogar.Application.Services.Implementations;
using Hogar.Application.Profiles;
using Microsoft.AspNetCore.Authentication.Cookies;
using MailKit.Net.Smtp;
using MimeKit;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query;
using Npgsql.EntityFrameworkCore.PostgreSQL;

var builder = WebApplication.CreateBuilder(args);



// Configuración de sesión
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Tiempo de expiración de la sesión
    options.Cookie.HttpOnly = true; // Cookies accesibles solo por HTTP
    options.Cookie.IsEssential = true; // Necesario para GDPR si aplica
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Solo en conexiones seguras
});



builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

 

// Configuración de la base de datos
builder.Services.AddDbContext<HogarContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresDataBase"));
    
    if (builder.Environment.IsDevelopment())
    {
        options.EnableSensitiveDataLogging();
    }
});

// Configuración de dependencias (Repository y Services)
// Repositories
builder.Services.AddTransient<IRepositoryNoticia, RepositoryNoticia>();
builder.Services.AddTransient<IRepositoryUsuario, RepositoryUsuario>();
builder.Services.AddTransient<IRepositoryTipoUsuario, RepositoryTipoUsuario>();

// Services 
builder.Services.AddTransient<IServiceNoticia, ServiceNoticia>();
builder.Services.AddTransient<ServiceEmail>();
builder.Services.AddTransient<IServiceUsuario, ServiceUsuario>();
builder.Services.AddTransient<IServiceTipoUsuario, ServiceTipoUsuario>();

// Configuración de AutoMapper
builder.Services.AddAutoMapper(config =>
{
    config.AddProfile<NoticiaProfile>();
    config.AddProfile<UsuarioProfile>();
    config.AddProfile<TipoUsuarioProfile>();
});

// Configuración de Serilog
var logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
    .Enrich.FromLogContext()
    .WriteTo.Console(LogEventLevel.Information)
    .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Information)
        .WriteTo.File(@"Logs\Info-.log", shared: true, encoding: Encoding.ASCII, rollingInterval: RollingInterval.Day))
    .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Debug)
        .WriteTo.File(@"Logs\Debug-.log", shared: true, encoding: Encoding.ASCII, rollingInterval: RollingInterval.Day))
    .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Warning)
        .WriteTo.File(@"Logs\Warning-.log", shared: true, encoding: Encoding.ASCII, rollingInterval: RollingInterval.Day))
    .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Error)
        .WriteTo.File(@"Logs\Error-.log", shared: true, encoding: Encoding.ASCII, rollingInterval: RollingInterval.Day))
    .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Fatal)
        .WriteTo.File(@"Logs\Fatal-.log", shared: true, encoding: Encoding.ASCII, rollingInterval: RollingInterval.Day))
    .CreateLogger();
builder.Host.UseSerilog(logger);

// Add services to the container.
builder.Services.AddControllersWithViews()
       .AddViewLocalization()
    .AddDataAnnotationsLocalization();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Index";  // Página de inicio de sesión
        options.AccessDeniedPath = "/Home/AccessDenied"; // Página de acceso denegado
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Expiración de sesión
    });

 

var app = builder.Build();

// Habilitar sesiones
app.UseSession(); // Asegúrate de agregar esto antes de UseAuthorization()

// Agregar autenticación con cookies


app.Use(async (context, next) =>
{
    var culture = context.Request.Cookies["Culture"] ?? "es-ES"; // Idioma predeterminado
    var cultureInfo = new System.Globalization.CultureInfo(culture);
    System.Globalization.CultureInfo.CurrentCulture = cultureInfo;
    System.Globalization.CultureInfo.CurrentUICulture = cultureInfo;

    await next.Invoke();
});


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseMiddleware<ErrorHandlingMiddleware>();
}

app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
