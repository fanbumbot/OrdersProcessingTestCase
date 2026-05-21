using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

using PaymentService.DataAccess.Postgres.Models;
using PaymentService.DataAccess.Postgres;
using PaymentService.WebAPI.UseCases;
using Confluent.Kafka;

namespace PaymentService.Tests.Unit.UseCases
{
    [TestFixture]
    public class GetPaymentByIdUseCaseTest
    {
        private AppDbContext _dbContext;
        private GetPaymentByIdHandler _handler;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDbGetPaymentByIdUseCase")
                .Options;

            _dbContext = new AppDbContext(options);
            var mapper = new PaymentMapper();
            _handler = new GetPaymentByIdHandler(_dbContext, mapper);
        }

        [Test]
        public async Task Handle()
        {
            var paymentId = 1;

            var model = new PaymentModel
            {
                Id = paymentId,
                OrderId = 1,
                Timestamp = DateTime.UtcNow,
                Price = 100,
                Status = false
            };
            await _dbContext.AddAsync(model);
            await _dbContext.SaveChangesAsync();


            var resultPaymentDto = await _handler.Handle(new GetPaymentByIdQuery(paymentId), CancellationToken.None);


            var paymentInDb = await _dbContext.Payments.FindAsync(paymentId);

            Assert.That(resultPaymentDto, Is.TypeOf<GetPaymentDto>());

            Assert.That(resultPaymentDto.Status, Is.EqualTo(paymentInDb.Status));
            Assert.That(resultPaymentDto.Price, Is.EqualTo(paymentInDb.Price));
            Assert.That(resultPaymentDto.DateCreate, Is.EqualTo(paymentInDb.Timestamp));
        }

        [Test]
        public async Task Handle_NoPayment()
        {
            var model = new PaymentModel
            {
                Id = 2,
                OrderId = 1,
                Timestamp = DateTime.UtcNow,
                Price = 100,
                Status = false
            };

            await _dbContext.AddAsync(model);
            await _dbContext.SaveChangesAsync();

            AsyncTestDelegate action = async () =>
                await _handler.Handle(new GetPaymentByIdQuery(3), CancellationToken.None);

            Assert.ThrowsAsync<PaymentNotFoundException>(action);
        }
    }
}
