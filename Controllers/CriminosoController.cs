using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PROJETOCNP.Context;
using PROJETOCNP.Models;
using PROJETOCNP.Services;

namespace PROJETOCNP.Controllers;

[ApiController]
[Route("[controller]")]
public class CriminosoController : ControllerBase
{
    private readonly OrganizadorContext _contexto;

    public CriminosoController(OrganizadorContext contexto)
    {
        _contexto = contexto;
    }

    [HttpGet("{id}")] // Público
    public IActionResult ObterPorId(int id)
    {
        var criminoso = _contexto.Criminosos.Find(id);

        if (criminoso == null)
            return NotFound();

        criminoso.Cpf = CpfFormatter.Formatar(criminoso.Cpf);

        return Ok(criminoso);
    }


    [HttpGet("ObterTodos")]
    public IActionResult ObterTodos()
    {
        var criminoso = _contexto.Criminosos.ToList();

        foreach (var i in criminoso)
        {
            i.Cpf = CpfFormatter.Formatar(i.Cpf);
        }

        return Ok(criminoso);
    }

    
    [HttpGet("ObterPorNome")] // Em dúvida se deixar público
    public IActionResult ObterPorNome(string nome)
    {
        var criminoso = _contexto.Criminosos.Where(x => x.NomeCompleto.Contains(nome)).ToList();

        if (nome == null)
            return NotFound();

        foreach (var i in criminoso)
        {
            i.Cpf = CpfFormatter.Formatar(i.Cpf);
        }

        return Ok(criminoso);
    }

    
    [HttpGet("ObterPorCPF")]
    public IActionResult ObterPorCPF(string cpf)
    {
        if (string.IsNullOrEmpty(cpf))
            return NotFound();

        if (string.IsNullOrWhiteSpace(cpf))
            throw new Exception("Preencha o valor a ser pesquisado.");

        var criminoso = _contexto.Criminosos.FirstOrDefault(c => c.Cpf == cpf);

        if (criminoso == null)
            return NotFound();

        criminoso.Cpf = CpfFormatter.Formatar(criminoso.Cpf);

        return Ok(criminoso);
    }

    
    [HttpGet("ObterPorStatus")]
    public IActionResult ObterPorStatus(EnumStatusCriminoso status)
    {
        var criminoso = _contexto.Criminosos.Where(x => x.Status == status);
        
         foreach (var i in criminoso)
        {
            i.Cpf = CpfFormatter.Formatar(i.Cpf);
        }

        return Ok(criminoso);
    }

    
    [HttpGet("ObterPorAntecedentes")]
    public IActionResult ObterPorAntecedentes(List<string> antecedentes)
    {
        //Resolver problema de cast
        var resultado = _contexto.Criminosos.Where(c => c.Antecedentes.Any(a => antecedentes.Contains(a)));

         foreach (var i in resultado)
        {
            i.Cpf = CpfFormatter.Formatar(i.Cpf);
        }

        return Ok(resultado);
    }

    // Verificar como fazer busca de TAGs para os Antecedentes ou uma Lista de Enum que possa ser agrupado mais de uma opção.

    
    [HttpPost]
    public IActionResult Incluir(Criminoso criminoso)
    {
        var usuarioID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        criminoso.CriadoPorUsuarioId = usuarioID;

        if (!User.IsInRole("Admin"))
        {
            criminoso.Status = EnumStatusCriminoso.Pendente;
        }
        
        var cpfFormatado = CpfFormatter.Formatar(criminoso.Cpf);

        if (criminoso.NomeCompleto == null)
            return BadRequest(new { Erro = "Campo Nome Completo não pode ser vazio" });
        if (cpfFormatado.Equals(Empty))
            return BadRequest(new { Erro = "Campo CPF não pode ser vazio" });
        if (criminoso.Antecedentes == null)
            return BadRequest(new { Erro = "Campo Antecedentes não pode ser vazio" });
        if (criminoso.Endereco == null)
            return BadRequest(new { Erro = "Campo Endereço não pode ser vazio" });

        _contexto.Criminosos.Add(criminoso);
        _contexto.SaveChanges();

        return CreatedAtAction(nameof(ObterPorId), new { id = criminoso.Id }, criminoso);
    }

    
    [HttpPut("{id}")]
    public IActionResult Atualizar(int id, Criminoso criminoso)
    {
        var criminosoBanco = _contexto.Criminosos.Find(id);

        if (criminosoBanco == null)
            return NotFound();

        criminosoBanco.NomeCompleto = criminoso.NomeCompleto;
        criminosoBanco.Cpf = CpfFormatter.Normalizar(criminoso.Cpf);
        criminosoBanco.Status = criminoso.Status;
        criminosoBanco.Antecedentes = criminoso.Antecedentes;
        criminosoBanco.Endereco = criminoso.Endereco;


        //_contexto.Criminosos.Update(criminoso);
        _contexto.SaveChanges();

        return Ok("Profile atualizado no Sistema com sucesso!");
    }

    
    [HttpDelete("{id}")]
    public IActionResult Deletar(int id)
    {
        var criminosoBanco = _contexto.Criminosos.Find(id);

        if (criminosoBanco == null)
            return NotFound();

        _contexto.Criminosos.Remove(criminosoBanco);
        _contexto.SaveChanges();

        return NoContent();
    }

    [HttpPatch("{id}/status")]
    public IActionResult AtualizarStatus(int id, EnumStatusCriminoso status)
    {
        
        var criminosoBanco = _contexto.Criminosos.Find(id);

        if (criminosoBanco == null)
            return NotFound();

        criminosoBanco.Status = status;
        _contexto.SaveChanges();

        return Ok("Status atualizado com sucesso!");
    }

}