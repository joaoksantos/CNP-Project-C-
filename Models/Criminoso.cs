using System.ComponentModel.DataAnnotations;

namespace PROJETOCNP.Models;

public class Criminoso
{
    [Key]
    public int Id { get; set; }

    public int CriadoPorUsuarioId { get; set; }

    public Usuario? CriadoPorUsuario { get; set; }

    [Required]
    [MaxLength(255)]
    public string NomeCompleto { get; set; } = default!;

    [Required]
    [MaxLength(11)]
    public string Cpf { get; set; } = default!;

    [Required]
    public EnumStatusCriminoso Status { get; set; }

    
    public EnumSituacaoPena SituacaoPena { get; set; }

    [Required]
    public List<string> Antecedentes { get; set; } = default!;

    public string Endereco { get; set; } = default!;
}
