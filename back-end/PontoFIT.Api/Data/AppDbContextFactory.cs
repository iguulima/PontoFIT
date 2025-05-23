using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace PontoFIT.Api.Data
{
	public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
	{
		public AppDbContext CreateDbContext(string[] args)
		{
			var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

			optionsBuilder.UseSqlServer("Server=DESKTOP-AQLQT41\\SQLEXPRESS;Database=PontoFITDb;Trusted_Connection=True;TrustServerCertificate=True");

			return new AppDbContext(optionsBuilder.Options);
		}
	}
}
