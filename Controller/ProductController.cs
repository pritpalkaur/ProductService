using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductService.Model;
using ProductService.Services;

namespace ProductService.Controller
{
        [ApiController]
        [Route("api/[controller]")]
        public class ProductController : ControllerBase
        {
            private readonly Services.IProductService _service;

            public ProductController(Services.IProductService service)
            {
                _service = service;
            }

            [HttpGet]
            public IActionResult GetAll() => Ok(_service.GetAll());

            [HttpGet("{id}")]
            public IActionResult GetById(int id)
            {

                var product = _service.GetById(id);
                return product == null ? NotFound() : Ok(product);
            }

            [HttpPost]
            public IActionResult Create([FromBody] Model.Product product)
            {
                if (product == null)
                    return BadRequest();

                _service.Add(product);
                return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
            }

            [HttpPut("{id}")]
            public IActionResult Update(int id, [FromBody] Model.Product updatedProduct)
            {
                var existing = _service.GetById(id);
                if (existing == null)
                    return NotFound();

                updatedProduct.Id = id;
                _service.Update(updatedProduct);
                return NoContent();
            }

            [HttpDelete("{id}")]
            public IActionResult Delete(int id)
            {
                var existing = _service.GetById(id);
                if (existing == null)
                    return NotFound();

                _service.Delete(id);
                return NoContent();
            }
        }
    
}
