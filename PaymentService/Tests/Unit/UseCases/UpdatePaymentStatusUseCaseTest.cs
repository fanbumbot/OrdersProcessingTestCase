using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

using PaymentService.DataAccess.Postgres.Models;
using PaymentService.DataAccess.Postgres;
using PaymentService.WebAPI.UseCases;
using Confluent.Kafka;
using PaymentService.Kafka;

using PaymentService.Tests.Unit.Mock;

namespace PaymentService.Tests.Unit.UseCases
{
    [TestFixture]
    public class UpdatePaymentStatusUseCaseTest
    {
        private AppDbContext _dbContext;
        private UpdatePaymentStatusHandler _handler;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDbUpdatePaymentStatusUseCase")
                .Options;

            _dbContext = new AppDbContext(options);
            var notificationService = new FakeNotificationService();
            _handler = new UpdatePaymentStatusHandler(_dbContext, notificationService);
        }

        [Test]
        public async Task Handle()
        {
            var paymentId = 122;
            var oldStatus = false;
            var newStatus = !oldStatus;

            var model = new PaymentModel
            {
                Id = paymentId,
                OrderId = 1,
                Timestamp = DateTime.UtcNow,
                Price = 100,
                Status = oldStatus
            };
            await _dbContext.AddAsync(model);
            await _dbContext.SaveChangesAsync();

            await _handler.Handle(new UpdatePaymentStatusCommand(paymentId, newStatus), CancellationToken.None);

            var paymentInDb = await _dbContext.Payments.FindAsync(paymentId);
            Assert.That(paymentInDb, Is.Not.Null);

            Assert.That(paymentInDb.Status, Is.EqualTo(newStatus));
        }
    }
}
