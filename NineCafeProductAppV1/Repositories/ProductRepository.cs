using Microsoft.EntityFrameworkCore;
using NineCafeProductAppV1.Data;
using NineCafeProductAppV1.Models;

namespace NineCafeProductAppV1.Repositories
{
    public class ProductRepository : IRepository<ProductPosting>
    {
        private readonly AppDbContext _context;
        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(ProductPosting entity)
        {
            await _context.productPostings.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _context.productPostings.FindAsync(id);
            if(product == null)
            {
                throw new KeyNotFoundException();
            }
            _context.productPostings.Remove(product);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ProductPosting>> GetAllAsync()
        {
            return await _context.productPostings.ToListAsync();
        }

        public async Task<ProductPosting?> GetByIdAsync(int id)
        {
            return await _context.productPostings.FindAsync(id);
            
            //FindAsync returns null if not found → no exception needed
            //if (product == null) throw new KeyNotFoundException();
            
        }

        public async Task UpdateAsync(ProductPosting entity)
        {
            _context.productPostings.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
