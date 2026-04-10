using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using YuyoDev.Domain.Entities;
using YuyoDev.Infrastructure.Persistence;
using YuyoDev.Application.Interfaces;
using YuyoDev.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

// --- 1. Agregar el servicio de CORS ---
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// 1. CONEXIÓN A BASE DE DATOS (PostgreSQL)
// Esto lee la cadena de conexión de tu appsettings.json (puerto 5439)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// 2. CONFIGURACIÓN DE IDENTITY
// Aquí es donde "casamos" a ApplicationUser con el DbContext
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.Password.RequireDigit = false;
        options.Password.RequiredLength = 6;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>() // <-- ¡Esta línea soluciona tu error 500!
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options => {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options => {
        options.TokenValidationParameters = new TokenValidationParameters {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]!)),
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero // Para que el token expire exacto cuando decimos
        };
    });

// Acá registramos el servicio de correos
builder.Services.AddTransient<IEmailService, LocalEmailService>();

// Habilita a la API a leer los datos de la petición actual (Headers, Cookies, etc.)
builder.Services.AddHttpContextAccessor();

// Registramos el servicio de Tenant. Usamos "AddScoped" porque queremos 
// que el TenantId se calcule una vez por cada petición HTTP que entra.
builder.Services.AddScoped<ITenantService, TenantService>();

// --- CONFIGURACIÓN DE HANGFIRE ---
builder.Services.AddHangfire(config => config
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UsePostgreSqlStorage(options =>
    options.UseNpgsqlConnection(builder.Configuration.GetConnectionString("DefaultConnection"))));

// Le decimos a la API que también funcione como un "Trabajador" que procesa tareas
builder.Services.AddHangfireServer();

// 3. SERVICIOS DE API Y SWAGGER
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ITokenService, TokenService>();

var app = builder.Build();

app.UseCors("AllowAll");

// 4. CONFIGURACIÓN DEL PIPELINE (Middleware)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "YuyoDev API V1");
        // Dejamos la ruta base como 'swagger'
    });
    app.UseHangfireDashboard();
}

// Importante: HTTP en Linux a veces es más estable para desarrollo local
//app.UseHttpsRedirection();

app.UseRouting();

// EL ORDEN AQUÍ ES CRÍTICO
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// --- SIEMBRA DE DATOS (Roles) ---
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        // Llamamos a nuestro seeder
        await YuyoDev.WebAPI.Seeders.RoleSeeder.SeedRolesAsync(services);
    }
    catch (Exception ex)
    {
        // Si algo falla al sembrar, lo vemos en la consola
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ocurrió un error al sembrar los roles en la base de datos.");
    }
}

app.Run(); // Esta es tu última línea actual

app.Run();