using Microsoft.EntityFrameworkCore;
using MicroService.Model;

namespace MicroService.Services
{
    public class ProductService : IProductService
    {
        private readonly ProductDbContext _context;

        public ProductService(ProductDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Product> GetAll() => _context.Products.ToList();

        public Product GetById(int id) => _context.Products.Find(id);

        public void Add(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public void Update(Product updatedProduct)
        {
            var existing = _context.Products.Find(updatedProduct.Id);
            if (existing != null)
            {
                existing.Name = updatedProduct.Name;
                existing.Price = updatedProduct.Price;
                _context.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            var product = _context.Products.Find(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
        }
    }
}