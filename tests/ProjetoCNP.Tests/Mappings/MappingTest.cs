

using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;
using PROJETOCNP.DTOs;
using PROJETOCNP.Mappings;
using PROJETOCNP.Models;
using Xunit;

namespace tests.ProjetoCNP.Tests.Mappings
{
    public class CriminosoProfileTest
    {
        private readonly IMapper _mapper;

        public CriminosoProfileTest()
        {
            var configExpression = new MapperConfigurationExpression();
            configExpression.AddProfile(new CriminosoProfile());

            var config = new MapperConfiguration(configExpression, NullLoggerFactory.Instance);

            _mapper = config.CreateMapper();
        }

        [Fact]
        public void AutoMapper_CrimiosoProfile_Configutration_IsValid()
        {
            _mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }

        [Fact]
        public void Map_CriminosoToCriminosoGetDto_MustMapFieldCorrectly()
        {
            var criminoso = new Criminoso
            {
                Id = 1,
                NomeCompleto = "John Doe",
                Cpf = "12345678901",
                Status = EnumStatusCriminoso.Pendente,
                SituacaoPena = EnumSituacaoPena.Desconhecido,
                Antecedentes = new List<string> { "Roubo", "Furto" },
                Endereco = "Rua A, 123"
            };

            var dto = _mapper.Map<CriminosoGetDto>(criminoso);
        }
    }
}