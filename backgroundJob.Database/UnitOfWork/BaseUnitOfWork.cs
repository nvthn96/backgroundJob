using backgroundJob.Database.Entity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace backgroundJob.Database.UnitOfWork
{
	public class BaseUnitOfWork<Context> : IUnitOfWork<Context> where Context : DbContext, new()
	{
		protected readonly Context _context = new();

		public async Task<IEnumerable<V>> RunProcedureAsync<V>(
			string procedure,
			IEnumerable<(string, object)> param,

			CancellationToken token = default
		) where V : BaseView
		{
			var query = procedure + " " + string.Join(", ", param.Select(item => item.Item1));
			var sqlParam = param.Select(item => new SqlParameter(item.Item1, item.Item2));

			var output = await _context.Set<V>().FromSqlRaw(query, sqlParam).ToListAsync(token);

			return output;
		}

		public async Task<V> RunTransactionAsync<U, V>(
			U unitOfWork,
			Func<U, CancellationToken, Task<V>> func,

			CancellationToken token = default
		) where U : IUnitOfWork<Context>
		{
			using (var transaction = _context.Database.BeginTransaction())
			{
				try
				{
					var result = await func.Invoke(unitOfWork, token);
					transaction.Commit();

					return result;
				}
				catch (Exception)
				{
					transaction.Rollback();
				}
			}

			return default!;
		}

		public async Task SaveChangesAsync(CancellationToken token = default)
		{
			await _context.SaveChangesAsync(token);
		}
	}
}
