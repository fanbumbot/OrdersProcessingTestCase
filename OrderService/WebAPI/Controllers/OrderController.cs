using Microsoft.AspNetCore.Mvc;

using OrderService.WebAPI.UseCases;

namespace OrderService.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderUseCase _orderService;

        public OrderController(IOrderUseCase orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("{order_id}", Name = "GetOrderInfo")]
        public IActionResult GetOrderInfo([FromRoute] int order_id)
        {
            var orderReport = _orderService.GetAsync(order_id);
            if (orderReport == null)
            {
                return NotFound();
            }
            return Ok(orderReport);
        }
    }
}
