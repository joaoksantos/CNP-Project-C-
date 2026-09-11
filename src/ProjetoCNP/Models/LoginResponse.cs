namespace PROJETOCNP.Models;

public class LoginResponse
{
    public bool Sucesso { get; set; }
    public string Mensagem { get; set; } = default!;
    public string Token { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}