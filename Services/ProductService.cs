using ProductService.Model;

namespace ProductService.Services
{
    public class ProductService : IProductService
    {
        private readonly List<Product> _products = new()
    {
        new Product { Id = 1, Name = "Laptop", Price = 999.99M },
        new Product { Id = 2, Name = "Phone", Price = 499.99M }
    };

        public IEnumerable<Product> GetAll() => _products;

        public Product GetById(int id) => _products.FirstOrDefault(p => p.Id == id);

        public void Add(Product product)
        {
            // Auto-increment ID if not set
            product.Id = _products.Any() ? _products.Max(p => p.Id) + 1 : 1;
            _products.Add(product);
        }

        public void Update(Product updatedProduct)
        {
            var index = _products.FindIndex(p => p.Id == updatedProduct.Id);
            if (index != -1)
            {
                _products[index] = updatedProduct;
            }
        }

        public void Delete(int id)
        {
            var product = GetById(id);
            if (product != null)
            {
                _products.Remove(product);
            }
        }
    }

}
