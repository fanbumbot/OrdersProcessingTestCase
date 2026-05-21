using MediatR;
using Microsoft.AspNetCore.Mvc;

using PaymentService.WebAPI.UseCases;

namespace PaymentService.WebAPI.Controllers
{
    /// <summary>
    /// Контроллер для данных об оплате заказов
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly ISender _mediator;

        public PaymentController(ISender mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Создание данных об оплате заказа
        /// </summary>
        /// <param name="createDto">DTO для создания</param>
        /// <param name="cancellationToken">Переменная для отмены задачи</param>
        /// <returns>200 - ок, 422 - ошибка валидации входных данных</returns>
        [HttpPost("create", Name = "CreatePayment")]
        public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentDto createDto, CancellationToken cancellationToken)
        {
            var paymentId = await _mediator.Send(new CreatePaymentCommand(createDto), cancellationToken);
            return Ok(paymentId);
        }

        /// <summary>
        /// Обновить данные о статусе оплаты заказа
        /// </summary>
        /// <param name="paymentId">Идентификатор данных об оплате заказа</param>
        /// <param name="status">Новый статус</param>
        /// <param name="cancellationToken">Переменная для отмены задачи</param>
        /// <returns>200 - ок, 404 - данные об оплате заказа не найдены, 422 - ошибка валидации входных данных</returns>
        [HttpPut("updateStatus/{paymentId}/{status}", Name = "UpdatePaymentStatus")]
        public async Task<IActionResult> UpdatePaymentStatus([FromRoute] int paymentId, [FromRoute] bool status, CancellationToken cancellationToken)
        {
            await _mediator.Send(new UpdatePaymentStatusCommand(paymentId, status), cancellationToken);
            return Ok();
        }

        /// <summary>
        /// Узнать данные об оплате заказа
        /// </summary>
        /// <param name="paymentId">Идентификатор данных об оплате заказа</param>
        /// <param name="cancellationToken">Переменная для отмены задачи</param>
        /// <returns>200 - ок, 404 - данные об оплате заказа не найдены, 422 - ошибка валидации входных данных</returns>
        [HttpGet("{paymentId}", Name = "GetPaymentById")]
        public async Task<IActionResult> GetPaymentInfo([FromRoute] int paymentId, CancellationToken cancellationToken)
        {
            var payment = await _mediator.Send(new GetPaymentByIdQuery(paymentId), cancellationToken);
            return Ok(payment);
        }
    }
}
