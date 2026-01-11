using SEP490.Infrastructure.Persistence;
using static SEP490.Application.Interfaces.IRepository;

namespace SEP490.Infrastructure.Repositories
{
    public class EfRepository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _ctx;
        private readonly ILoggingService _loggingService;

        public EfRepository(AppDbContext ctx, ILoggingService loggingService)
        {
            _ctx = ctx;
            _loggingService = loggingService;
        }

        public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            try
            {
                await _ctx.Set<T>().AddAsync(entity, cancellationToken);
                await _ctx.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                await _loggingService.LogErrorAsync($"Error adding entity of type {typeof(T).Name}: {ex.Message}", "AddAsync");
                throw;
            }
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
