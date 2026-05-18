using OrderService.DataAccess.Postgres.Models;
using OrderService.DataAccess.Postgres;
using Microsoft.EntityFrameworkCore;

namespace OrderService.WebAPI.UseCases
{
    public interface IOrderUseCase
    {
        public OrderReport? GetAsync(int order_id);
    }

    public class OrderUseCase : IOrderUseCase
    {
        AppDbContext _context;
        OrderMapper _mapper;
        public OrderUseCase(AppDbContext context, OrderMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public OrderReport? GetAsync(int order_id)
        {
            var model = _context.Orders.FirstOrDefault(o => o.Id == order_id);
            return _mapper.MapToReport(model);
        }
    }
}
