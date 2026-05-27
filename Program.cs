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

builder.Services.AddAutoMapper(cfg =>{
    cfg.LicenseKey = "eyJhbGciOiJSUzI1NiIsImtpZCI6Ikx1Y2t5UGVubnlTb2Z0d2FyZUxpY2Vuc2VLZXkvYmJiMTNhY2I1OTkwNGQ4OWI0Y2IxYzg1ZjA4OGNjZjkiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL2x1Y2t5cGVubnlzb2Z0d2FyZS5jb20iLCJhdWQiOiJMdWNreVBlbm55U29mdHdhcmUiLCJleHAiOiIxODEwOTQ0MDAwIiwiaWF0IjoiMTc3OTQyNjY0MiIsImFjY291bnRfaWQiOiIwMTllNGUxNmVlMGQ3OWJiOWM4N2RkYTlkMzI1MTk3NyIsImN1c3RvbWVyX2lkIjoiY3RtXzAxa3M3MWZqcmR3OWg5YWRnOXBiaGJhM3BqIiwic3ViX2lkIjoiLSIsImVkaXRpb24iOiIwIiwidHlwZSI6IjIifQ.1KTzuH3w6_NxX5YyIdt3sB1xjMdFl0fv3mM3XROyjUt21qM2UVh4ySuAZ86YrpEZPwzg-BjGbkkv_8M2BrXFT8P3NUHRh7VkoqTQ2Grxhnw1rvGPEBPvWsgVEb0aepkX0jBeoztlm-roImu-TOH03q96c0_t8KX2ip7PFHqDS63Um7ehnU9sThOW3EbGr6Kw3tBDcosIWegmOe7Zv63OU3c_2elArI1lVmWtF_ran0k-WSjuSbG7BZUCbCyufVPkpMPLx3u2Qq0GcwSR1v0zX7DmCqWNMuUlZsOGq5vClJo2IkJZGq32Gf7RYa9U4TSpOY_sElKO1MTVoFmuaMS-Ag";
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

//app.MapGet("/", () => "Hello World!");
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

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
