using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using OrderService.DataAccess.Postgres;
using OrderService.WebAPI.Controllers;
using OrderService.WebAPI.UseCases;

using OrderService.Tests.Unit.Mock;

namespace OrderService.Tests.Unit.Controllers
{
    [TestFixture]
    public class OrderControllerTest
    {
        private FakeMediator _mediator;
        private OrderController _controller;

        [SetUp]
        public void SetUp()
        {
            _mediator = new FakeMediator();
            _controller = new OrderController(_mediator);
        }

        [Test]
        public async Task Handle_Create()
        {
            var orderId = 123;

            var dto = new CreateOrderDto
            {
                ProductId = 1,
                Amount = 1,
                EmailClient = "test@mail.ru",
                PhoneNumber = "+70000000000",
                Price = 100
            };

            _mediator.SetResult(orderId); // Use case return value = orderId


            var result = await _controller.CreateOrder(dto, CancellationToken.None);


            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var createdResult = result as OkObjectResult;
            Assert.That(createdResult!.Value, Is.EqualTo(orderId));
        }

        [Test]
        public async Task Handle_Delete()
        {
            _mediator.SetResult(null);


            var result = await _controller.DeleteOrder(2, CancellationToken.None);


            Assert.That(result, Is.InstanceOf<OkResult>());
        }

        [Test]
        public async Task Handle_Get()
        {
            var getDto = new GetOrderDto
            {
                ProductId = 1,
                Amount = 1,
                EmailClient = "test@mail.ru",
                PhoneNumber = "+70000000000",
                Price = 100
            };

            _mediator.SetResult(getDto);


            var result = await _controller.GetOrderInfo(2, CancellationToken.None);


            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var createdResult = result as OkObjectResult;
            Assert.That(createdResult!.Value, Is.EqualTo(getDto));
        }
    }
}
