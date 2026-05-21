using MediatR;
using Microsoft.AspNetCore.Mvc;

using PaymentService.WebAPI.UseCases;

namespace PaymentService.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly ISender _mediator;

        public PaymentController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("create", Name = "CreatePayment")]
        public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentDto createDto, CancellationToken cancellationToken)
        {
            var paymentId = await _mediator.Send(new CreatePaymentCommand(createDto), cancellationToken);
            return Ok(paymentId);
        }

        [HttpPut("updateStatus/{paymentId}/{status}", Name = "UpdatePaymentStatus")]
        public async Task<IActionResult> UpdatePaymentStatus([FromRoute] int paymentId, [FromRoute] bool status, CancellationToken cancellationToken)
        {
            await _mediator.Send(new UpdatePaymentStatusCommand(paymentId, status), cancellationToken);
            return Ok();
        }

        [HttpGet("{paymentId}", Name = "GetPaymentById")]
        public async Task<IActionResult> GetPaymentInfo([FromRoute] int paymentId, CancellationToken cancellationToken)
        {
            var payment = await _mediator.Send(new GetPaymentByIdQuery(paymentId), cancellationToken);
            return Ok(payment);
        }
    }
}
