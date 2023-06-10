using backgroundJob.Custom.FileWatcher.Entity;
using Microsoft.EntityFrameworkCore;

namespace backgroundJob.Custom.FileWatcher.Database
{
	public class FileWatcherContext : DbContext
	{
		private readonly string connectionStr = @"Server=.\SQLEXPRESS;Database=BackgroundJob;Integrated Security=true;TrustServerCertificate=True;";
		private readonly string migrationTable = @"__MigrationsHistory";
		private readonly string schema = "FW";

		public FileWatcherContext() { }

		public FileWatcherContext(DbContextOptions<FileWatcherContext> options) : base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.HasDefaultSchema(schema);
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			base.OnConfiguring(optionsBuilder);
			optionsBuilder.UseSqlServer(connectionStr, d => d.MigrationsHistoryTable(migrationTable, schema));
		}

		public DbSet<FWPath> Path { get; set; }
		public DbSet<FWEvent> Event { get; set; }
	}
}
