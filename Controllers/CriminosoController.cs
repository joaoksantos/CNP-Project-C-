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
using PROJETOCNP.DTOs;
using PROJETOCNP.Mappings;
using AutoMapper;

namespace PROJETOCNP.Controllers;

[ApiController]
[Route("[controller]")]
public class CriminosoController : ControllerBase
{
    private readonly OrganizadorContext _contexto;

    private readonly IMapper _mapper;

    public CriminosoController(IMapper mapper, OrganizadorContext contexto)
    {
        _mapper = mapper;
        _contexto = contexto;
    }

    [HttpGet("{id}")] // Público
    public async Task<IActionResult> ObterPorId(int id)
    {
        var criminoso = await _contexto.Criminosos.FindAsync(id);

        if (criminoso == null)
            return NotFound();

        var dto = _mapper.Map<CriminosoGetDto>(criminoso);

        criminoso.Cpf = CpfFormatter.Formatar(criminoso.Cpf);

        return Ok(dto);
    }


    [HttpGet("ObterTodos")]
    public async Task<IActionResult> ObterTodos()
    {
        var criminoso = await _contexto.Criminosos.ToListAsync();

        var dtos = _mapper.Map<List<CriminosoGetDto>>(criminoso);

        foreach (var i in dtos)
        {
            i.Cpf = CpfFormatter.Formatar(i.Cpf);
        }

        return Ok(dtos);
    }

    
    [HttpGet("ObterPorNome")] // Em dúvida se deixar público
    public async Task<IActionResult> ObterPorNome(string nome)
    {
        if(string.IsNullOrWhiteSpace(nome))
            return BadRequest(new { Error = "Informe um nome para a pesquisa."});
        
        var criminoso = await _contexto.Criminosos
        .Where(x => x.NomeCompleto.Contains(nome))
        .ToListAsync();

        var dtos = _mapper.Map<List<CriminosoGetDto>>(criminoso);
        foreach (var i in dtos)
        {
            i.Cpf = CpfFormatter.Formatar(i.Cpf);
        }

        return Ok(dtos);
    }

    
    [HttpGet("ObterPorCPF")]
    public async Task<IActionResult> ObterPorCPF(string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf))
            return BadRequest(new { Error = "Informe um CPF para a pesquisa."});

        var cpfNormalizado = CpfFormatter.Normalizar(cpf);

        var criminoso = await _contexto.Criminosos
            .FirstOrDefaultAsync(c => c.Cpf == cpfNormalizado);

        if (criminoso == null)
            return NotFound();

        var dto = _mapper.Map<CriminosoGetDto>(criminoso);

        dto.Cpf = CpfFormatter.Formatar(criminoso.Cpf);

        return Ok(dto);
    }

    
    [HttpGet("ObterPorStatus")]
    public async Task<IActionResult> ObterPorStatus(EnumStatusCriminoso status)
    {
        var criminoso = await _contexto.Criminosos
            .Where(x => x.Status == status)
            .ToListAsync();
        
        var dtos = _mapper.Map<List<CriminosoGetDto>>(criminoso);        
         foreach (var i in dtos)
        {
            i.Cpf = CpfFormatter.Formatar(i.Cpf);
        }

        return Ok(dtos);
    }

    
    [HttpGet("ObterPorAntecedentes")]
    public async Task<IActionResult> ObterPorAntecedentes(List<string> antecedentes)
    {
        if(antecedentes == null || !antecedentes.Any())
            return BadRequest(new { Error = "Informe pelo menos um antecedente para a pesquisa."});
        var criminoso = await _contexto.Criminosos
            .Where(c => c.Antecedentes.Any(a => antecedentes.Contains(a)))
            .ToListAsync();

        var dtos = _mapper.Map<List<CriminosoGetDto>>(criminoso);
        foreach (var i in dtos)
        {
            i.Cpf = CpfFormatter.Formatar(i.Cpf);
        }

        return Ok(dtos);
    }

    // Verificar como fazer busca de TAGs para os Antecedentes ou uma Lista de Enum que possa ser agrupado mais de uma opção.

    
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Incluir(CriminosoCreateDto criminosoDto)
    {
        var criminoso = _mapper.Map<Criminoso>(criminosoDto);

        if (!User.IsInRole("Admin"))
        {
            criminoso.Status = EnumStatusCriminoso.Pendente;
        }

        var cpfFormatado = CpfFormatter.Formatar(criminoso.Cpf);

        if (criminoso.NomeCompleto == null)
            return BadRequest(new { Erro = "Campo Nome Completo não pode ser vazio" });
        if (cpfFormatado.Equals(string.Empty))
            return BadRequest(new { Erro = "Campo CPF não pode ser vazio" });
        if (criminoso.Antecedentes == null)
            return BadRequest(new { Erro = "Campo Antecedentes não pode ser vazio" });
        if (criminoso.Endereco == null)
            return BadRequest(new { Erro = "Campo Endereço não pode ser vazio" });

        await _contexto.Criminosos.AddAsync(criminoso);
        await _contexto.SaveChangesAsync();

        return CreatedAtAction(nameof(ObterPorId), new { id = criminoso.Id }, criminoso);
    }

    
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Atualizar(int id, CriminosoUpdateDto criminosoDto)
    {
        if (criminosoDto == null)
            return BadRequest(new { Erro = "Payload inválido." });

        var criminosoBanco = await _contexto.Criminosos.FindAsync(id);
        if (criminosoBanco == null)
            return NotFound();

        if (!string.IsNullOrWhiteSpace(criminosoDto.NomeCompleto))
            criminosoBanco.NomeCompleto = criminosoDto.NomeCompleto;

        if (!string.IsNullOrWhiteSpace(criminosoDto.Cpf))
            criminosoBanco.Cpf = CpfFormatter.Normalizar(criminosoDto.Cpf);

        criminosoBanco.Status = criminosoDto.Status;
        criminosoBanco.SituacaoPena = criminosoDto.SituacaoPena;

        if (criminosoDto.Antecedentes != null && criminosoDto.Antecedentes.Any())
            criminosoBanco.Antecedentes = criminosoDto.Antecedentes;

        if (!string.IsNullOrWhiteSpace(criminosoDto.Endereco))
            criminosoBanco.Endereco = criminosoDto.Endereco;

        await _contexto.SaveChangesAsync();

        return Ok(new { Mensagem = "Profile atualizado no sistema com sucesso!" });
    }

    
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Deletar(int id)
    {
        var criminosoBanco = await _contexto.Criminosos.FindAsync(id);

        if (criminosoBanco == null)
            return NotFound();

        _contexto.Criminosos.Remove(criminosoBanco);
        await _contexto.SaveChangesAsync();

        return NoContent();
    }

    [HttpPatch("{id}/status")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AtualizarStatus(int id,[FromBody] AtualizarStatusCriminosoDto dto)
    {
        
        var criminosoBanco = await _contexto.Criminosos.FindAsync(id);

        if (criminosoBanco == null)
            return NotFound();

        criminosoBanco.Status = dto.Status;
        
        await _contexto.SaveChangesAsync();

        return Ok("Status atualizado com sucesso!");
    }

}