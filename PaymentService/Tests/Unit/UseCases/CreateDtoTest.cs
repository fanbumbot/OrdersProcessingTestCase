using NUnit.Framework;
using PaymentService.Tests.Unit.Mock;
using PaymentService.WebAPI.Controllers;
using PaymentService.WebAPI.UseCases;

using FluentValidation.TestHelper;

namespace PaymentService.Tests.Unit.UseCases
{
    [TestFixture]
    public class CreateDtoTest
    {
        CreatePaymentDtoValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _validator = new CreatePaymentDtoValidator();
        }

        [Test]
        public async Task Handle_NoErrors()
        {
            var dto = new CreatePaymentDto
            {
                OrderId = 1,
                Price = 1,
            };

            var result = _validator.Validate(dto);

            Assert.That(result.Errors.Any(e => e.PropertyName == "OrderId"), Is.EqualTo(false));
            Assert.That(result.Errors.Any(e => e.PropertyName == "Price"), Is.EqualTo(false));
        }

        [Test]
        public async Task Handle_PriceEqualsZero()
        {
            var dto = new CreatePaymentDto
            {
                OrderId = 1,
                Price = 0,
            };

            var result = _validator.Validate(dto);

            Assert.That(result.Errors.Any(e => e.PropertyName == "OrderId"), Is.EqualTo(false));
            Assert.That(result.Errors.Any(e => e.PropertyName == "Price"), Is.EqualTo(true));
        }

        [Test]
        public async Task Handle_PriceLessThanZero()
        {
            var dto = new CreatePaymentDto
            {
                OrderId = 1,
                Price = -100,
            };

            var result = _validator.Validate(dto);

            Assert.That(result.Errors.Any(e => e.PropertyName == "OrderId"), Is.EqualTo(false));
            Assert.That(result.Errors.Any(e => e.PropertyName == "Price"), Is.EqualTo(true));
        }
    }
}
