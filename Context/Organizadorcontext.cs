using Microsoft.EntityFrameworkCore;
using PROJETOCNP.Models;

namespace PROJETOCNP.Context;

public class OrganizadorContext : DbContext
{
    public OrganizadorContext(DbContextOptions<OrganizadorContext> options) : base(options)
    {

    }
    
    public DbSet<Criminoso> Criminosos { get; set; }

    public DbSet<Usuario> Usuarios { get; set; }
}