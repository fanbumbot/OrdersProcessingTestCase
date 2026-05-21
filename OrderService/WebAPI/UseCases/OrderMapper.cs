using Riok.Mapperly.Abstractions;
using OrderService.DataAccess.Postgres.Models;

namespace OrderService.WebAPI.UseCases
{
    [Mapper]
    public partial class OrderMapper
    {
        [MapperIgnoreTarget(nameof(OrderModel.Id))]
        public partial OrderModel MapCreateDtoToModel(CreateOrderDto order);

        [MapperIgnoreSource(nameof(OrderModel.Id))]
        public partial GetOrderDto? MapModelToGetDto(OrderModel? order);
    }
}
