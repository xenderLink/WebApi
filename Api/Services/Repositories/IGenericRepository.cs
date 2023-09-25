namespace Store.Services.Repositories;

public interface IGenericRepository<T> 
        where T : class
{
    public Task<T> GetByIdAsync(object id, CancellationToken cancellationToken = default);
    public Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
    public Task CreateAsync(T entity, CancellationToken cancellationToken = default);
    public Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
    public Task DeleteAsync(object id, CancellationToken cancellationToken = default);                
}