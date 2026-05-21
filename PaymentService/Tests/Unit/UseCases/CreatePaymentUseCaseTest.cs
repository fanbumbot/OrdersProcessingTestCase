using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using PaymentService.DataAccess.Postgres;
using PaymentService.WebAPI.UseCases;

namespace PaymentService.Tests.Unit.UseCases
{
    [TestFixture]
    public class CreatePaymentUseCaseTest
    {
        private AppDbContext _dbContext;
        private CreatePaymentHandler _handler;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDbCreatePaymentUseCase")
                .Options;

            _dbContext = new AppDbContext(options);
            var mapper = new PaymentMapper();
            _handler = new CreatePaymentHandler(_dbContext, mapper);
        }

        [Test]
        public async Task Handle()
        {
            var dto = new CreatePaymentDto
            {
                OrderId = 1,
                Price = 100
            };


            var paymentId = await _handler.Handle(new CreatePaymentCommand(dto), CancellationToken.None);


            Assert.That(paymentId, Is.TypeOf<int>());

            var paymentInDb = await _dbContext.Payments.FindAsync(paymentId);

            Assert.That(paymentInDb.OrderId, Is.EqualTo(dto.OrderId));
            Assert.That(paymentInDb.Price, Is.EqualTo(dto.Price));
        }
    }
}
