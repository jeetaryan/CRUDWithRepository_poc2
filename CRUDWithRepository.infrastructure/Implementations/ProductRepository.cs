using CRUDWithRepository.Core;
using CRUDWithRepository.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDWithRepository.Infrastructure.Implementations
{
    public class ProductRepository : IProductRepository
    {
        private readonly MyAppDbContext _context;

        public ProductRepository(MyAppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            var products = await _context.products.ToListAsync();
            return products;
        }

        public async Task<Product> GetById(int id)
        {
            return await _context.products.FindAsync(id);
        }

        public async Task Add(Product model)
        {
            await _context.products.AddAsync(model);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Product model)
        {
            try
            {
                var product = await _context.products.SingleOrDefaultAsync(x => x.Id == model.Id);
                if (product != null)
                {
                    product.ProductName = model.ProductName;
                    product.Price = model.Price;
                    product.Qty = model.Qty;
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

    }
}
