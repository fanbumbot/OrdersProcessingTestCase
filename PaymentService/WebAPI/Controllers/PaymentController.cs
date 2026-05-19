using Microsoft.AspNetCore.Mvc;

using PaymentService.WebAPI.UseCases;

namespace PaymentService.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentUseCases _paymentService;

        public PaymentController(IPaymentUseCases paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("/create", Name = "CreatePayment")]
        public IActionResult CreatePayment([FromBody] PaymentCreateDto createDto)
        {
            var paymentId = _paymentService.Create(createDto);
            return Ok(paymentId);
        }

        [HttpGet("{paymentId}", Name = "GetPaymentInfo")]
        public IActionResult GetPaymentInfo([FromRoute] int paymentId)
        {
            var paymentReport = _paymentService.Get(paymentId);
            if (paymentReport == null)
            {
                return NotFound();
            }
            return Ok(paymentReport);
        }
    }
}
