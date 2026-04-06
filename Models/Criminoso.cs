using System.ComponentModel.DataAnnotations;

namespace PROJETOCNP.Models;

public class Criminoso
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(255)]
    public string NomeCompleto { get; set; } = default!;

    [Required]
    [MaxLength(11)]
    public string Cpf { get; set; } = default!;

    [Required]
    public EnumStatusCriminoso Status { get; set; }

    [Required]
    public List<string> Antecedentes { get; set; } = default!;
}
