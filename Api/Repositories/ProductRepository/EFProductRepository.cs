using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Store.Data;
using Store.Models.DTO;
using Store.Models.Entities;

namespace Store.Repositories;

internal sealed class ProductRepository : IProductRepository
{
    private readonly StoreDbContext _context;
    private readonly IMapper _mapper;

    public ProductRepository(StoreDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Product> GetByIdAsync(object id, CancellationToken cancellationToken)
    {
        var product = await _context.products.FindAsync(id, cancellationToken);

        return product is null ? default(Product) : product;
    }

    public async Task<int> GetCategoryId(string category, CancellationToken cancellationToken) =>
        await _context.sub_categories.Where(c => c.Name == category).Select(c => c.Id).FirstOrDefaultAsync((cancellationToken)); 
    
    public async Task<IEnumerable<Product>> GetByCategoryAsync(int id, CancellationToken cancellationToken) =>
        await _context.products.Where(p => p.Category.Id == id).ToListAsync(cancellationToken);
    
    public async Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken) => await _context.products.ToListAsync(cancellationToken);
    
    public async Task CreateAsync(Product product, CancellationToken cancellationToken)
    {
        await _context.products.AddAsync(product, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Product product, CancellationToken cancellationToken)
    {
        _context.products.Update(product);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(object id, CancellationToken cancellationToken) => 
           await _context.products.Where(p => p.Id == (long)id).ExecuteDeleteAsync(cancellationToken);
}