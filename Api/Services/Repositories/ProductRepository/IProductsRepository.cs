using Store.Models.Entities;

namespace Store.Services.Repositories;

public interface IProductRepository : IGenericRepository<Product>
{
    public Task<int> GetCategoryId(string category, CancellationToken cancellationToken = default);
    public Task<IEnumerable<Product>> GetByCategoryAsync(int id, CancellationToken cancellationToken = default);
}