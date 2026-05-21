using Riok.Mapperly.Abstractions;
using PaymentService.DataAccess.Postgres.Models;

namespace PaymentService.WebAPI.UseCases
{
    [Mapper]
    public partial class PaymentMapper
    {
        [MapperIgnoreTarget(nameof(PaymentModel.Id))]
        [MapperIgnoreTarget(nameof(PaymentModel.Status))]
        public partial PaymentModel MapCreateDtoToModel(CreatePaymentDto payment, DateTime timestamp);

        [MapperIgnoreSource(nameof(PaymentModel.Id))]
        [MapperIgnoreSource(nameof(PaymentModel.OrderId))]
        [MapProperty(nameof(PaymentModel.Timestamp), nameof(GetPaymentDto.DateCreate))]
        public partial GetPaymentDto? MapModelToGetDto(PaymentModel? payment);
    }
}
