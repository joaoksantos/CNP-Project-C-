using PROJETOCNP.Models;
using System.ComponentModel.DataAnnotations;

namespace PROJETOCNP.DTOs
{
    public class CriminosoGetDto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string NomeCompleto { get; set; } = default!;

        [Required]
        [MaxLength(11)]
        public string Cpf { get; set; } = default!;

        public EnumStatusCriminoso Status { get; set; }

        [Required]
        public EnumSituacaoPena SituacaoPena { get; set; } = default!;

        [Required]
        public List<string> Antecedentes { get; set; } = default!;

        public string Endereco { get; set; } = default!;
    }
}
