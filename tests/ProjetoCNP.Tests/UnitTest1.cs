using Moq;
using Xunit;
using System.Security.Claims;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using PROJETOCNP.Services;

namespace ProjetoCNP.Tests.Service;

public class JwtServiceTest
{

        private IConfiguration CreateConfigurationFake(
            string secretKey = "06882ab40f1ab09af83f2f88178743328f895e16683da85d7b7761964ac62f2c", /*my_secret_key_12345*/
            string emissor = "ProjetoCNP",
            string audiencia = "ProjetoCNPUsers",
            string expiracaoMinutos = "60"
        ){
            var inMemorySettings = new Dictionary<string, string?>
            {
                {"Jwt:SecretKey", secretKey},
                {"Jwt:Emissor", emissor},
                {"Jwt:Audiencia", audiencia},
                {"Jwt:ExpiracaoMinutos", expiracaoMinutos}
            };
            
            return new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
        }

        [Fact]
        public void GenerateToken_MustBeNotEmpty_WhenValidUserAndClaims()
        {
            // Arrange
            var config = CreateConfigurationFake();
            var jwtService = new JwtService(config);

            int usuarioId = 1;
            string email = "joao@example.com";
            string role = "Admin";

            string tokenString = jwtService.GerarToken(usuarioId, email, role);

            Assert.NotNull(tokenString);
            Assert.NotEmpty(tokenString);
        }

        [Fact]
        public void MustBeThrownException_WhenUserDataAreInvalid()
        {
            //Arange
            var config = CreateConfigurationFake();
            var jwtService = new JwtService(config);

            int usuarioId = 1;
            string email = ""; // Email inválido
            string role = "Admin";

            //Act
            Assert.Throws<ArgumentException>(() => jwtService.GerarToken(usuarioId, email, role));
        }

        [Fact]
        public void MustBeThrownException_WhenSecretKeyIsMissing()
        {
            // Arrange
            var inMemorySettings = new Dictionary<string, string?>(); // Sem as chaves do Jwt
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            // Act & Assert (Garante que a sua validação no construtor lança a exceção esperada)
            Assert.Throws<ArgumentNullException>(() => new JwtService(config));
        }
    
}