using backgroundJob.Custom.ApiChecking.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;

namespace backgroundJob.Custom.ApiChecking.Database
{
	public class ApiCheckingContext : DbContext
	{
		private readonly string connectionStr = @"Server=.\SQLEXPRESS;Database=BackgroundJob;Integrated Security=true;TrustServerCertificate=True;";
		private readonly string migrationTable = @"__MigrationsHistory";
		private readonly string schema = "AC";

		public ApiCheckingContext() { }

		public ApiCheckingContext(DbContextOptions<ApiCheckingContext> options) : base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.HasDefaultSchema(schema);
			modelBuilder.Ignore<IEnumerable<KeyValuePair<string, string>>>();

			var enumerableComparer = new ValueComparer<IEnumerable<KeyValuePair<string, string>>>(
					(c1, c2) => c1.SequenceEqual(c2),
					c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
					c => c.ToList());

			modelBuilder.Entity<ACUrl>()
				.Property(url => url.Headers).HasConversion(
					v => JsonConvert.SerializeObject(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }),
					v => JsonConvert.DeserializeObject<IEnumerable<KeyValuePair<string, string>>>(
						v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore })
					)
				.Metadata
				.SetValueComparer(enumerableComparer);

			modelBuilder.Entity<ACUrl>()
				.Property(url => url.Parameters).HasConversion(
					v => JsonConvert.SerializeObject(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }),
					v => JsonConvert.DeserializeObject<IEnumerable<KeyValuePair<string, string>>>(
						v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore })
					)
				.Metadata
				.SetValueComparer(enumerableComparer);
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			base.OnConfiguring(optionsBuilder);
			optionsBuilder.UseSqlServer(connectionStr, d => d.MigrationsHistoryTable(migrationTable, schema));
		}

		public DbSet<ACUrl> Url { get; set; }
		public DbSet<ACData> Data { get; set; }
	}
}
