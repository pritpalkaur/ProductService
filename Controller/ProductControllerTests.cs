using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using ProductService.Controller;
using ProductService.Services;
using ProductService.Model;
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
}