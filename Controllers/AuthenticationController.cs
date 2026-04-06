using Microsoft.AspNetCore.Mvc;
using PROJETOCNP.Models;
using PROJETOCNP.Services;

namespace PROJETOCNP.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly AuthenticationService _authenticationService;

    public AuthenticationController(AuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Senha))
        {
            return BadRequest(new { mensagem = "Email e senha são obrigatórios." });
        }

        var (sucesso, mensagem, token, role) = _authenticationService.Login(request.Email, request.Senha);

        if (!sucesso)
        {
            return Unauthorized(new { mensagem });
        }
        return Ok(new LoginResponse
        {
            Sucesso = sucesso,
            Mensagem = mensagem,
            Token = token,
            Role = role
        });
    }

    [HttpPost("register")]
    public IActionResult Registrar([FromBody] LoginRequest request)
    {
        if (_authenticationService.CriarUsuario(request.Email, request.Senha))
        {
            return Ok(new { mensagem = "Usuário registrado com sucesso." });
        }

        return BadRequest(new { mensagem = "Email já está em uso." });
    }
}