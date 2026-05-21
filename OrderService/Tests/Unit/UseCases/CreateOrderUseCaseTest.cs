using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using OrderService.DataAccess.Postgres;
using OrderService.WebAPI.UseCases;

namespace OrderService.Tests.Unit.UseCases
{
    [TestFixture]
    public class CreateOrderUseCaseTest
    {
        private AppDbContext _dbContext;
        private CreateOrderHandler _handler;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDbCreateOrderUseCase")
                .Options;

            _dbContext = new AppDbContext(options);
            var mapper = new OrderMapper();
            _handler = new CreateOrderHandler(_dbContext, mapper);
        }

        [Test]
        public async Task Handle()
        {
            var dto = new CreateOrderDto
            {
                ProductId = 1,
                Amount = 1,
                EmailClient = "test@mail.ru",
                PhoneNumber = "+70000000000",
                Price = 100
            };


            var orderId = await _handler.Handle(new CreateOrderCommand(dto), CancellationToken.None);


            Assert.That(orderId, Is.TypeOf<int>());

            var orderInDb = await _dbContext.Orders.FindAsync(orderId);

            Assert.That(orderInDb.ProductId, Is.EqualTo(dto.ProductId));
            Assert.That(orderInDb.Amount, Is.EqualTo(dto.Amount));
            Assert.That(orderInDb.EmailClient, Is.EqualTo(dto.EmailClient));
            Assert.That(orderInDb.PhoneNumber, Is.EqualTo(dto.PhoneNumber));
            Assert.That(orderInDb.Price, Is.EqualTo(dto.Price));
        }
    }
}
