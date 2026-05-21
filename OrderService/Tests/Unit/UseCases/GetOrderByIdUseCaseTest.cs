using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

using OrderService.DataAccess.Postgres.Models;
using OrderService.DataAccess.Postgres;
using OrderService.WebAPI.UseCases;
using Confluent.Kafka;

namespace OrderService.Tests.Unit.UseCases
{
    [TestFixture]
    public class GetOrderByIdUseCaseTest
    {
        private AppDbContext _dbContext;
        private GetOrderByIdHandler _handler;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDbGetOrderByIdUseCase")
                .Options;

            _dbContext = new AppDbContext(options);
            var mapper = new OrderMapper();
            _handler = new GetOrderByIdHandler(_dbContext, mapper);
        }

        [Test]
        public async Task Handle()
        {
            var orderId = 1;

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


            var resultOrderDto = await _handler.Handle(new GetOrderByIdQuery(orderId), CancellationToken.None);


            var orderInDb = await _dbContext.Orders.FindAsync(orderId);

            Assert.That(resultOrderDto, Is.TypeOf<GetOrderDto>());

            Assert.That(resultOrderDto.EmailClient, Is.EqualTo(orderInDb.EmailClient));
            Assert.That(resultOrderDto.ProductId, Is.EqualTo(orderInDb.ProductId));
            Assert.That(resultOrderDto.Price, Is.EqualTo(orderInDb.Price));
            Assert.That(resultOrderDto.Amount, Is.EqualTo(orderInDb.Amount));
            Assert.That(resultOrderDto.PhoneNumber, Is.EqualTo(orderInDb.PhoneNumber));
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
                await _handler.Handle(new GetOrderByIdQuery(3), CancellationToken.None);

            Assert.ThrowsAsync<OrderNotFoundException>(action);
        }
    }
}
