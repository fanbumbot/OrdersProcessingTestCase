using MediatR;
using Microsoft.AspNetCore.Mvc;

using OrderService.WebAPI.UseCases;
using System.Threading;

namespace OrderService.WebAPI.Controllers
{
    /// <summary>
    /// REST API контроллер для заказов
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly ISender _mediator;

        public OrderController(ISender mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Запрос на создание нового заказа
        /// </summary>
        /// <param name="createDto">DTO для создания заказа</param>
        /// <param name="cancellationToken">Переменная для отмены задачи</param>
        /// <returns>200 - ок, 422 - ошибка валидации входных данных</returns>
        [HttpPost("create", Name = "CreateOrder")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto createDto, CancellationToken cancellationToken)
        {
            var orderId = await _mediator.Send(new CreateOrderCommand(createDto), cancellationToken);
            return Ok(orderId);
        }

        /// <summary>
        /// Запрос на удаление заказа по идентификатору
        /// </summary>
        /// <param name="orderId">Идентификатор заказа</param>
        /// <param name="cancellationToken">Переменная для отмены задачи</param>
        /// <returns>200 - ок, 404 - заказ не найден, 422 - ошибка валидации входных данных</returns>
        [HttpDelete("{orderId}", Name = "DeleteOrder")]
        public async Task<IActionResult> DeleteOrder([FromRoute] int orderId, CancellationToken cancellationToken)
        {
            await _mediator.Send(new DeleteOrderCommand(orderId), cancellationToken);
            return Ok();
        }

        /// <summary>
        /// Запрос на получение информации о заказе по идентификатору
        /// </summary>
        /// <param name="orderId">Идентификатор заказа</param>
        /// <param name="cancellationToken">Переменная для отмены задачи</param>
        /// <returns>200 - ок, 404 - заказ не найден, 422 - ошибка валидации входных данных</returns>
        [HttpGet("{orderId}", Name = "GetOrderById")]
        public async Task<IActionResult> GetOrderInfo([FromRoute] int orderId, CancellationToken cancellationToken)
        {
            var orderReport = await _mediator.Send(new GetOrderByIdQuery(orderId), cancellationToken);
            return Ok(orderReport);
        }
    }
}
