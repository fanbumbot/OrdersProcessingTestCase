using OrderService.DataAccess.Postgres.Models;
using OrderService.DataAccess.Postgres;
using Microsoft.EntityFrameworkCore;

namespace OrderService.WebAPI.UseCases
{
    public interface IOrderUseCases
    {
        public int Create(OrderCreateDto createDto);
        public void Delete(int OrderId);
        public OrderGetDto? Get(int orderId);
    }

    public class OrderUseCases : IOrderUseCases
    {
        AppDbContext _context;
        OrderMapper _mapper;
        public OrderUseCases(AppDbContext context, OrderMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public int Create(OrderCreateDto createDto)
        {
            OrderModel model = _mapper.MapCreateDtoToModel(createDto);
            _context.Orders.Add(model);
            _context.SaveChanges();
            var modelId = model.Id;
            return modelId;
        }

        public void Delete(int orderId)
        {
            var model = _context.Orders.Find(orderId);
            if (model == null)
            {
                throw new OrderNotFoundException(orderId);
            }
            _context.Orders.Remove(model);
            _context.SaveChanges();
        }

        public OrderGetDto? Get(int orderId)
        {
            var model = _context.Orders.Find(orderId);
            if (model == null)
            {
                throw new OrderNotFoundException(orderId);
            }
            return _mapper.MapModelToGetDto(model);
        }
    }
}
