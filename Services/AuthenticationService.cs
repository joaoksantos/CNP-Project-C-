using PROJETOCNP.Context;
using PROJETOCNP.Models;
using System.Security.Cryptography;
using System.Text;

namespace PROJETOCNP.Services;

public class AuthenticationService
{
    private readonly OrganizadorContext _context;
    private readonly JwtService _jwtService;

    public AuthenticationService(OrganizadorContext context, JwtService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
    }

    public (bool Success, string Message, string Token, string Role) Login(string email, string senha)
    {
        var usuario = _context.Usuarios.FirstOrDefault(u => u.Email == email);
        //var senha = _context.Usuarios.FirstOrDefault(u => u.Senha == senha);
        if (usuario == null)
        {
            return (false, "Usuário não encontrado.", string.Empty,string.Empty);
        }
        if (!usuario.Ativo)
        {
            return (false, "Usuário inativo.", string.Empty,string.Empty);
        }

        if (!VerificarSenha(senha, usuario.SenhaHash))
        {
            return (false, "Senha incorreta.", string.Empty,string.Empty);
        }

        var token = _jwtService.GerarToken(usuario.Id, usuario.Email, usuario.Role);
        return (true, "Login realizado com sucesso.", token, usuario.Role);
    }

    public bool CriarUsuario(string email, string senha, string role = "Usuario")
    {
        if (_context.Usuarios.Any(u => u.Email == email))
        {
            return false; // Email já existe
        }

        var usuario = new Usuario
        {
            Email = email,
            SenhaHash = HashSenha(senha),
            Role = role,
            Ativo = true
        };

        _context.Usuarios.Add(usuario);
        _context.SaveChanges();
        return true;
    }

    private string HashSenha(string senha)
    {
        using (var sha256 = SHA256.Create())
        {
            var senhaHash = sha256.ComputeHash(Encoding.UTF8.GetBytes(senha));
            return Convert.ToBase64String(senhaHash);
        }
    }

    public static bool VerificarSenha(string senha, string senhaHash)
    {
        var hashSenha = System.Convert.ToBase64String(SHA256.Create()
        .ComputeHash(Encoding.UTF8.GetBytes(senha)));
        return hashSenha == senhaHash;
    }
}