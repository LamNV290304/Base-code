using SEP490.Infrastructure.Persistence;
using static SEP490.Application.Interfaces.IRepository;

namespace SEP490.Infrastructure.Repositories
{
    public class EfRepository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _ctx;
        public EfRepository(AppDbContext ctx) => _ctx = ctx;

        public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _ctx.Set<T>().AddAsync(entity, cancellationToken);
            await _ctx.SaveChangesAsync(cancellationToken);
        }

        public async Task<T?> GetAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _ctx.Set<T>().FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            _ctx.Set<T>().Update(entity);
            await _ctx.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
        {
            _ctx.Set<T>().Remove(entity);
            await _ctx.SaveChangesAsync(cancellationToken);
        }

        public IQueryable<T> Query() => _ctx.Set<T>();
    }
}
