using Microsoft.EntityFrameworkCore;
using PV521_BooksShop.DAL.Entities;

namespace PV521_BooksShop.DAL.Repositories
{
    public class GenericRepository<TEntity>
        where TEntity : class, IBaseEntity
    {
        private readonly AppDbContext _context;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
        }

        public IQueryable<TEntity> GetAll()
        {
            return _context.Set<TEntity>().AsNoTracking();
        }

        public virtual async Task<TEntity?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await _context.Set<TEntity>().FirstOrDefaultAsync(e => e.Id == id, ct);
        }

        public async Task<bool> CreateAsync(TEntity entity, CancellationToken ct = default)
        {
            await _context.Set<TEntity>().AddAsync(entity, ct);
            var res = await _context.SaveChangesAsync(ct);
            return res > 0;
        }

        public async Task<int> CreateRangeAsync(IEnumerable<TEntity> entities, CancellationToken ct = default)
        {
            var tasks = _context.Set<TEntity>().Select(b => _context.AddAsync(b, ct).AsTask());
            await Task.WhenAll(tasks);
            return await _context.SaveChangesAsync(ct);
        }

        public async Task<bool> UpdateAsync(TEntity entity, CancellationToken ct = default)
        {
            entity.Updated = DateTime.UtcNow;
            _context.Set<TEntity>().Update(entity);
            var res = await _context.SaveChangesAsync(ct);
            return res > 0;
        }

        public async Task<bool> DeleteAsync(TEntity entity, CancellationToken ct = default)
        {
            _context.Set<TEntity>().Remove(entity);
            var res = await _context.SaveChangesAsync(ct);
            return res > 0;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
        {
            var entity = await GetByIdAsync(id);
            return entity != null && await DeleteAsync(entity, ct);
        }
    }
}
