using PROJETOCNP.Models;

namespace PROJETOCNP.DTOs
{
    public class CriminosoCreateDto
    {
        public string NomeCompleto { get; set; } = default!;

        public EnumStatusCriminoso Status { get; set; }

        public EnumSituacaoPena SituacaoPena { get; set; } = default!;

        public List<string> Antecedentes { get; set; } = default!;

        public string Endereco { get; set; } = default!;
    }
}