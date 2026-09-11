namespace PROJETOCNP.Models;

public class Usuario
{
    public int Id { get; set; }
    public string Email { get; set; } = default!;
    public string SenhaHash { get; set; } = default!;
    public string Role { get; set; } = "Usuario";
    public bool Ativo { get; set; } = true;
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
}
