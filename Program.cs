using MicroService.Model;
using MicroService.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext
builder.Services.AddDbContext<ProductDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ProductsDbConnection")));
builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("OrdersDbConnection")));

builder.Services.AddScoped<IOrderService, OrderService>();

// Register services
builder.Services.AddScoped<IProductService, MicroService.Services.ProductService>();
builder.Services.AddControllers(); // ✅ Required for controller mapping
//// Load ocelot.json
//builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
//Log.Logger = new LoggerConfiguration()
//    .WriteTo.File("Logs/ocelot.log")
//    .CreateLogger();

builder.Host.UseSerilog();

// Register Ocelot
//builder.Services.AddOcelot();

// Swagger setup
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ProductService API",
        Version = "v1"
    });
});

var app = builder.Build();

// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthorization(); // Optional if you're using auth
app.MapControllers();   // ✅ Maps your controller endpoints

app.UseHttpsRedirection();
app.UseHsts();


app.Run();