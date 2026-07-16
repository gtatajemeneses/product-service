using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.Entities;

namespace ProductService.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _context;
    public ProductRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _context.Products.ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _context.Products.FindAsync(id);
    }

    public async Task<Product> CreateAsync(Product product)
    {
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task<Product> UpdateAsync(Product product)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        // Usamos ExecuteDeleteAsync (Disponible en EF Core 7+) 
        // Es mucho más eficiente que traer el objeto a memoria (FindAsync + Remove)
        int rowsAffected = await _context.Products
            .Where(p => p.Id == id)
            .ExecuteDeleteAsync();

        return rowsAffected > 0;
    }
}
