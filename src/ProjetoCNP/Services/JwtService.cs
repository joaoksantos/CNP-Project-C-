using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace PROJETOCNP.Services;

public class JwtService
{
    private readonly string _secretKey;
    private readonly string _emissor;
    private readonly string _audiencia;
    private readonly int _expiracaoMinutos;

    public JwtService(IConfiguration configuration)
    {
        _secretKey = configuration["Jwt:SecretKey"] ?? throw new ArgumentNullException("Jwt:SecretKey");
        _emissor = configuration["Jwt:Emissor"] ?? "ProjetoCNP";
        _audiencia = configuration["Jwt:Audiencia"] ?? "ProjetoCNPUsers";
        _expiracaoMinutos = int.Parse(configuration["Jwt:ExpiracaoMinutos"] ?? "60");
    }

    public string GerarToken(int usuarioId,string email, string role)
    {
        var chave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
        var credenciais = new SigningCredentials(chave, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, usuarioId.ToString()),
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Role, role)
        };

        var token = new JwtSecurityToken(
            issuer: _emissor,
            audience: _audiencia,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_expiracaoMinutos),
            signingCredentials: credenciais
        );

        return new JwtSecurityTokenHandler().WriteToken(token);

    }
}