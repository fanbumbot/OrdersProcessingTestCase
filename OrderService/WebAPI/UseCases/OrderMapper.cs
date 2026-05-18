using Riok.Mapperly.Abstractions;
using OrderService.DataAccess.Postgres.Models;

namespace OrderService.WebAPI.UseCases
{
    [Mapper]
    public partial class OrderMapper
    {
        [MapperIgnoreSource(nameof(OrderModel.Id))]
        public partial OrderReport? MapToReport(OrderModel? order);
    }
}
