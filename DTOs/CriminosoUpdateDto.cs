using PROJETOCNP.Models;

namespace PROJETOCNP.DTOs
{
    public class CriminosoUpdateDto
    {
        public string? NomeCompleto { get; set; }

        public string? Cpf { get; set; }

        public EnumStatusCriminoso Status { get; set; }

        public EnumSituacaoPena SituacaoPena { get; set; }

        public List<string>? Antecedentes { get; set; }
        
        public string? Endereco { get; set; }
    }
}