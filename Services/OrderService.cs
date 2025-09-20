using global::OrderService.Data;
using global::ProductService.Model;
using Microsoft.EntityFrameworkCore;

namespace ProductService.Services
{
    public class OrderService : IOrderService
    {
        private readonly OrderDbContext _context;

        public OrderService(OrderDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Order> GetAll() => _context.Orders.Include(o => o.Items).ToList();

        public Order? GetById(int id) => _context.Orders.Include(o => o.Items).FirstOrDefault(o => o.Id == id);

        public void Add(Order order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
        }

        public void Update(Order order)
        {
            var existing = _context.Orders.Include(o => o.Items).FirstOrDefault(o => o.Id == order.Id);
            if (existing != null)
            {
                existing.CustomerName = order.CustomerName;
                existing.OrderDate = order.OrderDate;
                existing.Items = order.Items;
                _context.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            var order = _context.Orders.Find(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                _context.SaveChanges();
            }
        }
    }
}