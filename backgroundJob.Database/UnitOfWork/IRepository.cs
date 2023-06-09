using System.Linq.Expressions;

namespace backgroundJob.Database.UnitOfWork
{
	public interface IRepository<T>
	{
		IEnumerable<T> GetAll();
		IEnumerable<T> GetAllTracking();
		IEnumerable<T> Filter(Expression<Func<T, bool>> condition);
		IEnumerable<T> FilterTracking(Expression<Func<T, bool>> condition);
		Task<T?> FindAsync(Expression<Func<T, bool>> condition, CancellationToken token = default);
		Task<T?> FindTrackingAsync(Expression<Func<T, bool>> condition, CancellationToken token = default);
		IEnumerable<T> TopPartition<TGroup, TKey>(
			Func<T, TGroup> groupSelector,
			Func<T, TKey> orderSelector);
		IEnumerable<T> TopPartitionTracking<TGroup, TKey>(
			Func<T, TGroup> groupSelector,
			Func<T, TKey> orderSelector);

		Task<int> CountAsync(CancellationToken token = default);
		Task<int> CountAsync(Expression<Func<T, bool>> condition, CancellationToken token = default);

		Task<T> AddAsync(T entity, CancellationToken token = default);
		Task<IEnumerable<Guid>> AddRangeAsync(IEnumerable<T> entities, CancellationToken token = default);
		Task<T?> UpdateAsync(T entity, CancellationToken token = default);
		Task<int> UpdateRangeAsync(IEnumerable<T> entities, CancellationToken token = default);
		Task<bool> DeleteAsync(Guid id, CancellationToken token = default);

		void Clear();
	}
}
