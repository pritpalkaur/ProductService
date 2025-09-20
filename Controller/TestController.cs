using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MicroService.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var dummyData = new
            {
                Id = 1,
                Name = "Test Product",
                Price = 99.99
            };

            return Ok(dummyData);
        }
    }
}
