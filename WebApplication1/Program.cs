using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using WebApplication1.Models.DB;
using WebApplication1.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using WebApplication1.Middleware;
using Serilog;
using WebApplication1.Connections.BD;
using WebApplication1.Utils;

var builder = WebApplication.CreateBuilder(args);

// Configuración de Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File(
        path: $"logs/log-{DateTime.Now:yyyyMMdd}.txt",
        rollingInterval: RollingInterval.Day,
        fileSizeLimitBytes: 10 * 1024 * 1024, // 10 MB
        rollOnFileSizeLimit: true,
        retainedFileCountLimit: null,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message}{NewLine}{Exception}")
    .CreateLogger();

builder.Host.UseSerilog();

// Configuración de JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    // Verificar si la clave secreta no es nula o vacía antes de usarla
    string? secretKey = builder.Configuration["Jwt:SecretKey"];
    if (string.IsNullOrEmpty(secretKey))
    {
        throw new InvalidOperationException("La clave secreta para JWT no está configurada en la configuración.");
    }

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
});

string corsPolicy = "CorsPolicy";

builder.Services.AddCors(options =>
{
    options.AddPolicy(corsPolicy,
    builder => builder
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod()
        .Build());
});


// Agregar servicios
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Monitoreo API", Version = "v1" });

    // Configuración de seguridad JWT en Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Encabezado de autorización JWT usando el esquema Bearer. \r\n\r\n Introduce 'Bearer' [espacio] y luego tu token en el campo de entrada a continuación.\r\n\r\nEjemplo: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<LdapService>();
builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<MenuService>();
builder.Services.AddScoped<ListaRestrictivaMetodos>();
builder.Services.AddSingleton<GeminiService>();
builder.Services.AddScoped<ITokenProvider, TokenProvider>();

// Configuración de la base de datos
string? connectionString = builder.Configuration.GetConnectionString("BDConexion");
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("La cadena de conexión 'BDConexion' no está configurada en la configuración.");
}
builder.Services.AddDbContext<MonitoreopyaContext>(options =>
    options.UseSqlServer(connectionString)
);
builder.Services.AddDbContext<MonitoreopyaContext>(options =>
    options.UseSqlServer(connectionString)
);

var app = builder.Build();

// Configuración del middleware
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseCors(corsPolicy);
//valida que el token no se haya revocado..
app.UseMiddleware<TokenRevocationMiddleware>();
//app.UseMiddleware<AuthorizationMiddleware>();

app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllers();

app.Run();
