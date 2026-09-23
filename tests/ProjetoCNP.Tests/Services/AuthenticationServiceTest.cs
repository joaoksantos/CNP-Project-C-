using PROJETOCNP.Services;
using Microsoft.EntityFrameworkCore;
using PROJETOCNP.Context;
using Xunit;
using Moq;
using Microsoft.Extensions.Configuration;
using PROJETOCNP.Models;



namespace ProjetoCNP.Tests.Service;

public class AuthenticationServiceTest
{
    private OrganizadorContext CriarContextoFake()
    {
        var options = new DbContextOptionsBuilder<OrganizadorContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new OrganizadorContext(options);
    }

    private JwtService CriarJwtSErviceFake()
    {
        var configuration = new Dictionary<string, string?>
        {
            { "Jwt:SecretKey", "63ecca73552eacf3055ed1e59807c8c4b9d9093af793d1a0e2fff1cbb33326ae" /*MinhaChaveSecreta1234567890*/ },
            { "Jwt:Emissor", "ProjetoCNP" },
            { "Jwt:Audiencia", "ProjetoCNPUsers" },
            { "Jwt:ExpiracaoMinutos", "60" }
        };

        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(configuration)
            .Build();
            
        return new JwtService(config);
    }

    // Teste quando usuario nao econtrado
    [Fact]
    public async Task Login_UserNotFound_ReturnFalse()
    {
        var context = CriarContextoFake();

        var jwtService = CriarJwtSErviceFake();

        var service = new AuthenticationService(
            context, 
            jwtService
        );
        
        var result = await service.Login(
            "teste@email.com",
            "senha123"
        );

        Assert.False(result.Success);
        Assert.Equal("Usuário não encontrado.", result.Message);
        Assert.Empty(result.Token);
        Assert.Empty(result.Role);
    }

    // Teste quando usuario esta inativo
    [Fact]
    public async Task Login_UserDeactivated_MustRetrunError()
    {
        var context = CriarContextoFake();
        var jwtService = CriarJwtSErviceFake();

        var service = new AuthenticationService(
            context,
            jwtService
        );

        await service.CriarUsuario(
            "teste@email.com",
            "123456"
        );

        // Desativar o usuário
        var usuario = await context.Usuarios.FirstOrDefaultAsync(u => u.Email == "teste@email.com");

        if (usuario == null) return;
        
        usuario.Ativo = false;

        await context.SaveChangesAsync();

        var result = await service.Login(
            "teste@email.com",
            "123456"
        );

        Assert.False(result.Success);
        Assert.Equal("Usuário inativo.", result.Message);
        Assert.Empty(result.Token);
        Assert.Empty(result.Role);
    }

    // Teste quando o usuario fornece a senha incorreta
    [Fact]
    public async Task Login_UserWithWrongPassword_MustRetrunError()
    {
        var context = CriarContextoFake();
        var jwtService = CriarJwtSErviceFake();

        var service = new AuthenticationService(
            context,
            jwtService
        );

        await service.CriarUsuario(
            "teste@email.com",
            "123456"
        );

        var result = await service.Login(
            "teste@email.com",
            "senhaErrada"
        );

        Assert.False(result.Success);
        Assert.Equal("Senha incorreta.", result.Message);
        Assert.Empty(result.Token);
        Assert.Empty(result.Role);
    }

    // Teste com credenciais corretas
    [Fact]
    public async Task Login_CorrectCredentials_MustReturnSuccess()
    {
        var context = CriarContextoFake();
        var jwtService = CriarJwtSErviceFake();

        var service = new AuthenticationService(
            context,
            jwtService
        );

        await service.CriarUsuario(
            "teste@email.com",
            "123456",
            "Admin"
        );

        var result = await service.Login(
            "teste@email.com",
            "123456"
        );

        Assert.True(result.Success);
        Assert.Equal("Login realizado com sucesso.", result.Message);
        Assert.False(string.IsNullOrEmpty(result.Token));
        Assert.Equal("Admin",result.Role);
    }
}