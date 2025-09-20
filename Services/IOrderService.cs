using ProductService.Model;

namespace ProductService.Services
{
    public interface IOrderService
    {
        IEnumerable<Order> GetAll();
        Order? GetById(int id);
        void Add(Order order);
        void Update(Order order);
        void Delete(int id);
    }

}
