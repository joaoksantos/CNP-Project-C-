using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PROJETOCNP.Context;
using PROJETOCNP.Services;
using PROJETOCNP.Mappings;


var builder = WebApplication.CreateBuilder(args);

var conexao = builder.Configuration.GetConnectionString("MinhaConexao");

builder.Services.AddDbContext<OrganizadorContext>(options =>
{
    options.UseMySql(conexao, ServerVersion.AutoDetect(conexao));
});

// Configuração do JWT
var secretKey = builder.Configuration["Jwt:SecretKey"]
    ?? throw new InvalidOperationException("A chave secreta do JWT não foi configurada.");
var chave = Encoding.ASCII.GetBytes(secretKey);

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(chave),
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Emissor"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audiencia"]
    };
});

builder.Services.AddAuthorization();
builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<AuthenticationService>();

var key_map = builder.Configuration.GetConnectionMapper("KeyMap")
builder.Services.AddAutoMapper(cfg =>{
    cfg.LicenseKey = key_map;
    cfg.AddProfile(new CriminosoProfile());
});

builder.Services.AddControllers();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Insira o token JWT no formato: Bearer {seu_token}"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();


app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<OrganizadorContext>();
    dbContext.Database.Migrate();

    var authService = scope.ServiceProvider.GetRequiredService<AuthenticationService>();

    if (!dbContext.Usuarios.Any())
    {
        authService.CriarUsuario("user@cnp.com", "admin@123", "Usuario");
    }
}

app.Run();
