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
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Senha))
        {
            return BadRequest(new { mensagem = "Email e senha são obrigatórios." });
        }

        var (sucesso, mensagem, token, role) = await _authenticationService.Login(request.Email, request.Senha);

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
    public async Task<IActionResult> Registrar([FromBody] LoginRequest request)
    {
        if(string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Senha))
        {
            return BadRequest(new { mensagem = "Email e senha são obrigatórios." });
        }
        if (await _authenticationService.CriarUsuario(request.Email, request.Senha))
        {
            return Ok(new { mensagem = "Usuário registrado com sucesso." });
        }

        return BadRequest(new { mensagem = "Email já está em uso." });
    }
}