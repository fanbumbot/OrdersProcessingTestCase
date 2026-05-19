using PaymentService.DataAccess.Postgres.Models;
using PaymentService.DataAccess.Postgres;
using Microsoft.EntityFrameworkCore;

namespace PaymentService.WebAPI.UseCases
{
    public interface IPaymentUseCases
    {
        public int Create(PaymentCreateDto createDto);
        public void UpdateStatus(int paymentId, bool status);
        public PaymentGetDto? Get(int paymentId);
    }

    public class PaymentUseCases : IPaymentUseCases
    {
        AppDbContext _context;
        PaymentMapper _mapper;
        public PaymentUseCases(AppDbContext context, PaymentMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public int Create(PaymentCreateDto createDto)
        {
            var timestamp = DateTime.UtcNow;
            PaymentModel model = _mapper.MapCreateDtoToModel(createDto, timestamp);
            _context.Payments.Add(model);
            _context.SaveChanges();
            var modelId = model.Id;
            return modelId;
        }

        public void UpdateStatus(int paymentId, bool status)
        {
            var model = _context.Payments.Find(paymentId);
            if (model == null)
            {
                throw new PaymentNotFoundException(paymentId);
            }
            model.Status = status;
            _context.SaveChanges();
        }

        public PaymentGetDto? Get(int paymentId)
        {
            var model = _context.Payments.Find(paymentId);
            if (model == null)
            {
                throw new PaymentNotFoundException(paymentId);
            }
            return _mapper.MapModelToGetDto(model);
        }
    }
}
