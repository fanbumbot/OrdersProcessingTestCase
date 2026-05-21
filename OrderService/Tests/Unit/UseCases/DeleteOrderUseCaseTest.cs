using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

using OrderService.DataAccess.Postgres.Models;
using OrderService.DataAccess.Postgres;
using OrderService.WebAPI.UseCases;
using Confluent.Kafka;

namespace OrderService.Tests.Unit.UseCases
{
    [TestFixture]
    public class DeleteOrderUseCaseTest
    {
        private AppDbContext _dbContext;
        private DeleteOrderHandler _handler;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDbDeleteOrderUseCase")
                .Options;

            _dbContext = new AppDbContext(options);
            var mapper = new OrderMapper();
            _handler = new DeleteOrderHandler(_dbContext, mapper);
        }

        [Test]
        public async Task Handle()
        {
            int orderId = 1;

            var model = new OrderModel
            {
                Id = orderId,
                ProductId = 1,
                Amount = 1,
                EmailClient = "test@mail.ru",
                PhoneNumber = "+70000000000",
                Price = 100
            };
            await _dbContext.AddAsync(model);
            await _dbContext.SaveChangesAsync();

            await _handler.Handle(new DeleteOrderCommand(orderId), CancellationToken.None);

            var orderInDb = await _dbContext.Orders.FindAsync(orderId);
            Assert.That(orderInDb, Is.Null);
        }

        [Test]
        public async Task Handle_NoOrder()
        {
            var model = new OrderModel
            {
                Id = 2,
                ProductId = 1,
                Amount = 1,
                EmailClient = "test@mail.ru",
                PhoneNumber = "+70000000000",
                Price = 100
            };
            await _dbContext.AddAsync(model);
            await _dbContext.SaveChangesAsync();

            AsyncTestDelegate action = async () =>
                await _handler.Handle(new DeleteOrderCommand(3), CancellationToken.None);

            Assert.ThrowsAsync<OrderNotFoundException>(action);
        }
    }
}
