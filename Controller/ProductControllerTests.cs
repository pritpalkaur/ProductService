using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using ProductService.Controller;
using MicroService.Services;
using MicroService.Model;
using System.Collections.Generic;

public class ProductControllerTests
{
    private readonly Mock<IProductService> _mockService;
    private readonly ProductController _controller;

    public ProductControllerTests()
    {
        _mockService = new Mock<IProductService>();
        _controller = new ProductController(_mockService.Object);
    }

    [Fact]
    public void GetAll_ReturnsOkResult_WithProductList()
    {
        // Arrange
        var products = new List<Product> {
            new Product { Id = 1, Name = "Laptop", Price = 999.99M },
            new Product { Id = 2, Name = "Phone", Price = 499.99M }
        };
        _mockService.Setup(s => s.GetAll()).Returns(products);

        // Act
        var result = _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsAssignableFrom<IEnumerable<Product>>(okResult.Value);
        Assert.Equal(2, ((List<Product>)returnValue).Count);
    }

    [Fact]
    public void GetById_ProductExists_ReturnsOkResult()
    {
        // Arrange
        var product = new Product { Id = 1, Name = "Laptop", Price = 999.99M };
        _mockService.Setup(s => s.GetById(1)).Returns(product);

        // Act
        var result = _controller.GetById(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<Product>(okResult.Value);
        Assert.Equal("Laptop", returnValue.Name);
    }

    [Fact]
    public void GetById_ProductNotFound_ReturnsNotFound()
    {
        // Arrange
        _mockService.Setup(s => s.GetById(99)).Returns((Product)null);

        // Act
        var result = _controller.GetById(99);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
public void Add_ValidProduct_ReturnsCreatedAtAction()
    {
        // Arrange
        var newProduct = new Product { Name = "something", Price = 399.99M };
        _mockService.Setup(s => s.Add(It.IsAny<Product>()));

        // Act
        var result = _controller.Create(newProduct);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        var returnValue = Assert.IsType<Product>(createdResult.Value);
        Assert.Equal("something", returnValue.Name);
    }

    [Fact]
    public void Update_ExistingProduct_ReturnsNoContent()
    {
        // Arrange
        var existingProduct = new Product { Id = 1, Name = "Laptop1", Price = 999.99M };
        _mockService.Setup(s => s.GetById(1)).Returns(existingProduct);
        _mockService.Setup(s => s.Update(It.IsAny<Product>()));

        var updatedProduct = new Product { Id = 1, Name = "Laptop Pro", Price = 1299.99M };

        // Act
        var result = _controller.Update(1, updatedProduct);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public void Update_ProductNotFound_ReturnsNotFound()
    {
        // Arrange
        _mockService.Setup(s => s.GetById(99)).Returns((Product)null);

        var updatedProduct = new Product { Id = 99, Name = "Ghost", Price = 0M };

        // Act
        var result = _controller.Update(99, updatedProduct);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void Delete_ExistingProduct_ReturnsNoContent()
    {
        // Arrange
        var product = new Product { Id = 2, Name = "Phone", Price = 499.99M };
        _mockService.Setup(s => s.GetById(2)).Returns(product);
        _mockService.Setup(s => s.Delete(2));

        // Act
        var result = _controller.Delete(2);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public void Delete_ProductNotFound_ReturnsNotFound()
    {
        // Arrange
        _mockService.Setup(s => s.GetById(100)).Returns((Product)null);

        // Act
        var result = _controller.Delete(100);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

}