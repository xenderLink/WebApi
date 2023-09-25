using Store.Models.Entities;

namespace Store.Services.Repositories;

public interface ISubCategoryRepository : IGenericRepository<SubCategory>

{
    public Task<short> GetParentCategoryId(string category, CancellationToken cancellationToken = default);
    public Task<IEnumerable<SubCategory>> GetByCategoryAsync(short id, CancellationToken cancellationToken = default);  
}