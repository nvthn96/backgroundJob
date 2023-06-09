using backgroundJob.Custom.ProcessTracking.Entity;
using backgroundJob.Infrastructure.View.AppSettings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace backgroundJob.Custom.ProcessTracking.Database
{
	public class ProcessContext : DbContext
	{
		//private readonly IConfiguration _configuration;
		private readonly IOptions<ConnectionStrings> optionConnectionString;

		public ProcessContext() { }

		public ProcessContext(
			DbContextOptions<ProcessContext> options,
			IOptions<ConnectionStrings> connectionStrings
			) : base(options)
		{
			optionConnectionString = connectionStrings;
		}
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.HasDefaultSchema("PT");
		}
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			base.OnConfiguring(optionsBuilder);
			optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=BackgroundJob;Integrated Security=true;TrustServerCertificate=True;");
		}

		public DbSet<PTProcess> Process { get; set; }
		public DbSet<PTTime> Time { get; set; }
	}
}
