using MediatR;
using Microsoft.AspNetCore.Mvc;

using OrderService.WebAPI.UseCases;
using System.Threading;

namespace OrderService.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly ISender _mediator;

        public OrderController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("create", Name = "CreateOrder")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto createDto, CancellationToken cancellationToken)
        {
            var orderId = await _mediator.Send(new CreateOrderCommand(createDto), cancellationToken);
            return Ok(orderId);
        }

        [HttpDelete("{orderId}", Name = "DeleteOrder")]
        public async Task<IActionResult> DeleteOrder([FromRoute] int orderId, CancellationToken cancellationToken)
        {
            await _mediator.Send(new DeleteOrderCommand(orderId), cancellationToken);
            return Ok();
        }

        [HttpGet("{orderId}", Name = "GetOrderById")]
        public async Task<IActionResult> GetOrderInfo([FromRoute] int orderId, CancellationToken cancellationToken)
        {
            var orderReport = await _mediator.Send(new GetOrderByIdQuery(orderId), cancellationToken);
            return Ok(orderReport);
        }
    }
}
