using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using PaymentService.DataAccess.Postgres;
using PaymentService.WebAPI.Controllers;
using PaymentService.WebAPI.UseCases;

using PaymentService.Tests.Unit.Mock;

namespace PaymentService.Tests.Unit.Controllers
{
    [TestFixture]
    public class PaymentControllerTest
    {
        private FakeMediator _mediator;
        private PaymentController _controller;

        [SetUp]
        public void SetUp()
        {
            _mediator = new FakeMediator();
            _controller = new PaymentController(_mediator);
        }

        [Test]
        public async Task Handle_Create()
        {
            var paymentId = 123;

            var dto = new CreatePaymentDto
            {
                OrderId = 1,
                Price = 100
            };

            _mediator.SetResult(paymentId); // Use case return value = paymentId


            var result = await _controller.CreatePayment(dto, CancellationToken.None);


            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var createdResult = result as OkObjectResult;
            Assert.That(createdResult!.Value, Is.EqualTo(paymentId));
        }

        [Test]
        public async Task Handle_UpdateTrue()
        {
            _mediator.SetResult(null);


            var result = await _controller.UpdatePaymentStatus(2, true, CancellationToken.None);


            Assert.That(result, Is.InstanceOf<OkResult>());
        }

        [Test]
        public async Task Handle_Get()
        {
            var getDto = new GetPaymentDto
            {
                Price = 100,
                Status = false,
                DateCreate = DateTime.UtcNow,
            };

            _mediator.SetResult(getDto);


            var result = await _controller.GetPaymentInfo(2, CancellationToken.None);


            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var createdResult = result as OkObjectResult;
            Assert.That(createdResult!.Value, Is.EqualTo(getDto));
        }
    }
}
