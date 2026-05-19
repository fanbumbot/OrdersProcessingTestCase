using Microsoft.AspNetCore.Mvc;

using OrderService.WebAPI.UseCases;

namespace OrderService.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderUseCases _orderService;

        public OrderController(IOrderUseCases orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("/create", Name = "CreateOrder")]
        public IActionResult CreateOrder([FromBody] OrderCreateDto createDto)
        {
            var orderId = _orderService.Create(createDto);
            return Ok(orderId);
        }

        [HttpDelete("{orderId}", Name = "DeleteOrder")]
        public IActionResult DeleteOrder([FromRoute] int orderId)
        {
            _orderService.Delete(orderId);
            return Ok();
        }

        [HttpGet("{orderId}", Name = "GetOrderInfo")]
        public IActionResult GetOrderInfo([FromRoute] int orderId)
        {
            var orderReport = _orderService.Get(orderId);
            return Ok(orderReport);
        }
    }
}
