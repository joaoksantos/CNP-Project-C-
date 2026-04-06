using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
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
public class UsuarioController : ControllerBase
{
    private readonly OrganizadorContext _contexto;

    public UsuarioController(OrganizadorContext contexto)
    {
        _contexto = contexto;
    }

    
    [HttpGet("ObterTodos")]
    public IActionResult ObterTodos()
    {
        var usuarios = _contexto.Usuarios.ToList();
        return Ok(usuarios);
    }
}