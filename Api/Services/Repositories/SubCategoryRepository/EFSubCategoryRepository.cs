using Microsoft.EntityFrameworkCore;
using Store.Data;
using Store.Models.DTO;
using Store.Models.Entities;

namespace Store.Services.Repositories;

public sealed class SubCategoryRepository : ISubCategoryRepository
{
    private readonly StoreDbContext _context;
    
    public SubCategoryRepository(StoreDbContext context) => _context = context;

    public async Task<SubCategory> GetByIdAsync(object id, CancellationToken cancellationToken)
    {
        var category = await _context.sub_categories.FindAsync(id, cancellationToken);

        return category is null ? default(SubCategory) : category;
    }

    public async Task<short> GetParentCategoryId(string category, CancellationToken cancellationToken) =>
        await _context.categories.Where(c => c.Name == category).Select(c => c.Id).FirstOrDefaultAsync(cancellationToken);

    public async Task<IEnumerable<SubCategory>> GetByCategoryAsync(short id, CancellationToken cancellationToken) =>
        await _context.sub_categories.Where(sc => sc.Category.Id == id).ToListAsync(cancellationToken);

    public async Task<IEnumerable<SubCategory>> GetAllAsync(CancellationToken cancellationToken) =>
        await _context.sub_categories.ToListAsync(cancellationToken);

    public async Task CreateAsync(SubCategory category, CancellationToken cancellationToken)
    {
        await _context.sub_categories.AddAsync(category, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(SubCategory category,  CancellationToken cancellationToken)
    {
        _context.sub_categories.Update(category);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(object id, CancellationToken cancellationToken) =>
           await _context.sub_categories.Where(c => c.Id == (int)id).ExecuteDeleteAsync(cancellationToken);
}