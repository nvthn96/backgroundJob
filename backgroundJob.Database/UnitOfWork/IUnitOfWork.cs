using backgroundJob.Database.Entity;
using Microsoft.EntityFrameworkCore;

namespace backgroundJob.Database.UnitOfWork
{
	public interface IUnitOfWork<Context> where Context : DbContext, new()
	{
		Task<IEnumerable<V>> RunProcedureAsync<V>(
			string procedure,
			IEnumerable<(string, object)> param,

			CancellationToken token = default
		) where V : BaseView;

		Task<V> RunTransactionAsync<U, V>(
			U unitOfWork,
			Func<U, CancellationToken, Task<V>> func,

			CancellationToken token = default
		) where U : IUnitOfWork<Context>;

		Task SaveChangesAsync(CancellationToken token = default);
	}
}
