using MicroService.Services;
using Microsoft.AspNetCore.Mvc;
using MicroService.Model;
using MicroService.Services;

namespace MicroService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _service;

        public OrderController(IOrderService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_service.GetAll());

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var order = _service.GetById(id);
            return order == null ? NotFound() : Ok(order);
        }

        [HttpPost]
        public IActionResult Create(Order order)
        {
            _service.Add(order);
            return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Order order)
        {
            if (id != order.Id) return BadRequest();
            var existing = _service.GetById(id);
            if (existing == null) return NotFound();

            _service.Update(order);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var existing = _service.GetById(id);
            if (existing == null) return NotFound();

            _service.Delete(id);
            return NoContent();
        }
    }
}
