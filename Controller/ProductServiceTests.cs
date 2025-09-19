using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ProductService.Controller
{
    using Xunit;
    using Microsoft.EntityFrameworkCore;
    using ProductService.Model;
    using ProductService.Services;
    using System.Collections.Generic;
    using System.Linq;

    public class ProductServiceTests
    {
        private ProductDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ProductDbContext>()
                .UseInMemoryDatabase(databaseName: "ProductTestDb")
                .Options;

            var context = new ProductDbContext(options);
            context.Database.EnsureDeleted(); // Reset between tests
            context.Database.EnsureCreated();
            return context;
        }

        [Fact]
        public void Add_Product_SavesToDatabase()
        {
            var context = GetInMemoryDbContext();
            var service = new ProductService(context);

            var product = new Product { Name = "Tablet123", Price = 499.99M };
            service.Add(product);

            var saved = context.Products.FirstOrDefault(p => p.Name == "Tablet123");
            Assert.NotNull(saved);
            Assert.Equal(499.99M, saved.Price);
        }

        [Fact]
        public void GetAll_ReturnsAllProducts()
        {
            var context = GetInMemoryDbContext();
            context.Products.AddRange(new List<Product>
        {
            new Product { Name = "Laptop", Price = 999.99M },
            new Product { Name = "Phone", Price = 499.99M }
        });
            context.SaveChanges();

            var service = new ProductService(context);
            var products = service.GetAll().ToList();

            Assert.Equal(2, products.Count);
        }

        [Fact]
        public void GetById_ReturnsCorrectProduct()
        {
            var context = GetInMemoryDbContext();
            var product = new Product { Name = "Laptop", Price = 999.99M };
            context.Products.Add(product);
            context.SaveChanges();

            var service = new ProductService(context);
            var result = service.GetById(product.Id);

            Assert.NotNull(result);
            Assert.Equal("Laptop", result.Name);
        }

        [Fact]
        public void Update_Product_ChangesPersisted()
        {
            var context = GetInMemoryDbContext();
            var product = new Product { Name = "Phone", Price = 499.99M };
            context.Products.Add(product);
            context.SaveChanges();

            var service = new ProductService(context);
            product.Name = "Smartphone";
            product.Price = 599.99M;
            service.Update(product);

            var updated = context.Products.Find(product.Id);
            Assert.Equal("Smartphone", updated.Name);
            Assert.Equal(599.99M, updated.Price);
        }

        [Fact]
        public void Delete_Product_RemovesFromDatabase()
        {
            var context = GetInMemoryDbContext();
            var product = new Product { Name = "Laptop", Price = 999.99M };
            context.Products.Add(product);
            context.SaveChanges();

            var service = new ProductService(context);
            service.Delete(product.Id);

            var deleted = context.Products.Find(product.Id);
            Assert.Null(deleted);
        }
    }
}
