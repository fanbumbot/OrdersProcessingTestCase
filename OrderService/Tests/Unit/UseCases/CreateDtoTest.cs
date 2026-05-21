using NUnit.Framework;
using OrderService.Tests.Unit.Mock;
using OrderService.WebAPI.Controllers;
using OrderService.WebAPI.UseCases;

using FluentValidation.TestHelper;

namespace OrderService.Tests.Unit.UseCases
{
    [TestFixture]
    public class CreateDtoTest
    {
        CreateOrderDtoValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _validator = new CreateOrderDtoValidator();
        }

        [Test]
        public async Task Handle_NoErrors()
        {
            var dto = new CreateOrderDto
            {
                ProductId = 1,
                Amount = 1,
                EmailClient = "test@mail.ru",
                PhoneNumber = "+70000000000",
                Price = 100
            };

            var result = _validator.Validate(dto);

            Assert.That(result.Errors.Any(e => e.PropertyName == "ProductId"), Is.EqualTo(false));
            Assert.That(result.Errors.Any(e => e.PropertyName == "Amount"), Is.EqualTo(false));
            Assert.That(result.Errors.Any(e => e.PropertyName == "EmailClient"), Is.EqualTo(false));
            Assert.That(result.Errors.Any(e => e.PropertyName == "PhoneNumber"), Is.EqualTo(false));
            Assert.That(result.Errors.Any(e => e.PropertyName == "Price"), Is.EqualTo(false));
        }

        [Test]
        public async Task Handle_AmountEqualsZero()
        {
            var dto = new CreateOrderDto
            {
                ProductId = 1,
                Amount = 0,
                EmailClient = "test@mail.ru",
                PhoneNumber = "+70000000000",
                Price = 100
            };

            var result = _validator.Validate(dto);

            Assert.That(result.Errors.Any(e => e.PropertyName == "Amount"), Is.EqualTo(true));
        }

        [Test]
        public async Task Handle_AmountLessThanZero()
        {
            var dto = new CreateOrderDto
            {
                ProductId = 1,
                Amount = -10,
                EmailClient = "test@mail.ru",
                PhoneNumber = "+70000000000",
                Price = 100
            };

            var result = _validator.Validate(dto);

            Assert.That(result.Errors.Any(e => e.PropertyName == "Amount"), Is.EqualTo(true));
        }

        [Test]
        public async Task Handle_WrongEmail()
        {
            var dto = new CreateOrderDto
            {
                ProductId = 1,
                Amount = 1,
                EmailClient = "testabc",
                PhoneNumber = "+70000000000",
                Price = 100
            };

            var result = _validator.Validate(dto);

            Assert.That(result.Errors.Any(e => e.PropertyName == "EmailClient"), Is.EqualTo(true));
        }

        [Test]
        public async Task Handle_WrongPhoneLenLong()
        {
            var dto = new CreateOrderDto
            {
                ProductId = 1,
                Amount = 1,
                EmailClient = "test@mail.ru",
                PhoneNumber = "+7000000000011993111",
                Price = 100
            };

            var result = _validator.Validate(dto);

            Assert.That(result.Errors.Any(e => e.PropertyName == "PhoneNumber"), Is.EqualTo(true));
        }

        [Test]
        public async Task Handle_WrongPhoneChars()
        {
            var dto = new CreateOrderDto
            {
                ProductId = 1,
                Amount = 1,
                EmailClient = "test@mail.ru",
                PhoneNumber = "+700000000d0",
                Price = 100
            };

            var result = _validator.Validate(dto);

            Assert.That(result.Errors.Any(e => e.PropertyName == "PhoneNumber"), Is.EqualTo(true));
        }

        [Test]
        public async Task Handle_PriceEqualsZero()
        {
            var dto = new CreateOrderDto
            {
                ProductId = 1,
                Amount = 1,
                EmailClient = "test@mail.ru",
                PhoneNumber = "+70000000000",
                Price = 0
            };

            var result = _validator.Validate(dto);

            Assert.That(result.Errors.Any(e => e.PropertyName == "Price"), Is.EqualTo(true));
        }

        [Test]
        public async Task Handle_PriceLessThanZero()
        {
            var dto = new CreateOrderDto
            {
                ProductId = 1,
                Amount = 1,
                EmailClient = "test@mail.ru",
                PhoneNumber = "+70000000000",
                Price = -100
            };

            var result = _validator.Validate(dto);

            Assert.That(result.Errors.Any(e => e.PropertyName == "Price"), Is.EqualTo(true));
        }
    }
}
