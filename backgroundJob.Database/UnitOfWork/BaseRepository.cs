using backgroundJob.Database.Entity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace backgroundJob.Database.UnitOfWork
{
	public class BaseRepository<T> : IRepository<T> where T : BaseEntity
	{
		private readonly DbContext _context;

		public BaseRepository(DbContext dbContext)
		{
			_context = dbContext;
		}

		public IEnumerable<T> GetAll()
		{
			var table = _context.Set<T>();
			var output = table.AsNoTracking();
			return output;
		}

		public IEnumerable<T> GetAllTracking()
		{
			var table = _context.Set<T>();
			var output = table;
			return output;
		}

		public IEnumerable<T> Filter(Expression<Func<T, bool>> condition)
		{
			var table = _context.Set<T>();
			var output = table.AsNoTracking().Where(condition);
			return output;
		}

		public IEnumerable<T> FilterTracking(Expression<Func<T, bool>> condition)
		{
			var table = _context.Set<T>();
			var output = table.Where(condition);
			return output;
		}

		public async Task<T?> FindAsync(Expression<Func<T, bool>> condition, CancellationToken token = default)
		{
			var table = _context.Set<T>();
			var output = await table.AsNoTracking().FirstOrDefaultAsync(condition, token);
			return output;
		}

		public async Task<T?> FindTrackingAsync(Expression<Func<T, bool>> condition, CancellationToken token = default)
		{
			var table = _context.Set<T>();
			var output = await table.FirstOrDefaultAsync(condition, token);
			return output;
		}

		public IEnumerable<T> TopPartition<TGroup, TKey>(
			Func<T, TGroup> groupSelector,
			Func<T, TKey> orderSelector)
		{
			var table = _context.Set<T>();
			var items = table.AsNoTracking();
			var topLastDetect = items
				.GroupBy(groupSelector, (key, g) => g.OrderByDescending(orderSelector))
				.Select(g => g.First());

			return topLastDetect;
		}

		public IEnumerable<T> TopPartitionTracking<TGroup, TKey>(
			Func<T, TGroup> groupSelector,
			Func<T, TKey> orderSelector)
		{
			var table = _context.Set<T>();
			var items = table;
			var topLastDetect = items
				.GroupBy(groupSelector, (key, g) => g.OrderByDescending(orderSelector))
				.Select(g => g.First());

			return topLastDetect;
		}

		public async Task<int> CountAsync(CancellationToken token = default)
		{
			var table = _context.Set<T>();
			var output = await table.AsNoTracking().CountAsync(token);
			return output;
		}

		public async Task<int> CountAsync(Expression<Func<T, bool>> condition, CancellationToken token = default)
		{
			var table = _context.Set<T>();
			var output = await table.AsNoTracking().Where(condition).CountAsync(token);
			return output;
		}

		public async Task<T> AddAsync(T entity, CancellationToken token = default)
		{
			var table = _context.Set<T>();

			entity.Id = entity.Id != Guid.Empty ? entity.Id : Guid.NewGuid();
			entity.CreatedOn = DateTime.UtcNow;

			var output = table.Add(entity);
			await _context.SaveChangesAsync(token);

			return output.Entity;
		}

		public async Task<IEnumerable<Guid>> AddRangeAsync(IEnumerable<T> entities, CancellationToken token = default)
		{
			var table = _context.Set<T>();

			var addedId = new List<Guid>();
			foreach (var entity in entities)
			{
				entity.Id = entity.Id != Guid.Empty ? entity.Id : Guid.NewGuid();
				entity.CreatedOn = DateTime.UtcNow;

				addedId.Add(entity.Id);
			}

			table.AddRange(entities);
			await _context.SaveChangesAsync(token);

			return addedId;
		}

		public async Task<T?> UpdateAsync(T entity, CancellationToken token = default)
		{
			if (entity.Id == Guid.Empty) return null;

			entity.ModifiedOn = DateTime.UtcNow;

			_context.Entry(entity).State = EntityState.Modified;
			await _context.SaveChangesAsync(token);

			return entity;
		}

		public async Task<int> UpdateRangeAsync(IEnumerable<T> entities, CancellationToken token = default)
		{
			var updated = entities.Where(e => e.Id != Guid.Empty);
			foreach (var entity in updated)
			{
				entity.ModifiedOn = DateTime.UtcNow;
				_context.Entry(entity).State = EntityState.Modified;
			}
			var output = await _context.SaveChangesAsync(token);

			return output;
		}

		public async Task<bool> DeleteAsync(Guid id, CancellationToken token = default)
		{
			var table = _context.Set<T>();
			var entity = await table.FindAsync(id);
			if (entity == null) return false;

			entity.IsDeleted = true;
			entity.DeletedOn = DateTime.Now;

			_context.Entry(entity).State = EntityState.Modified;
			await _context.SaveChangesAsync(token);

			return true;
		}

		public void Clear()
		{
			var table = _context.Set<T>();
			table.RemoveRange(table);
		}
	}
}
