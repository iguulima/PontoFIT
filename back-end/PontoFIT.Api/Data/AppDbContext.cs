using Microsoft.EntityFrameworkCore;
using PontoFIT.Api.Models;

namespace PontoFIT.Api.Data
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{
		}

		public DbSet<Usuario> Usuarios { get; set; }
		public DbSet<MarcacaoPonto> MarcacoesPonto { get; set; }
	}
}