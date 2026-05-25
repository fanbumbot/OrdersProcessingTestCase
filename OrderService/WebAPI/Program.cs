using FluentValidation;
using MediatR;
using Refit;
using Microsoft.EntityFrameworkCore;
using OrderService.DataAccess.Postgres;
using OrderService.WebAPI.Client;
using OrderService.WebAPI.UseCases;
using OrderService.Kafka;

namespace OrderService.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

            builder.Services.AddControllers();

            builder.Services.AddExceptionHandler<ExceptionHandler>();

            builder.Services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
                cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
            });

            builder.Services.AddProblemDetails();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddSingleton<IKafkaProducer, KafkaProducer>();
            builder.Services.AddSingleton<INotificationService, NotificationService>();

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));
            builder.Services.AddSingleton<OrderMapper>();

            builder.Services.AddRefitClient<IPaymentServiceClient>()
                .ConfigureHttpClient(c =>
                {
                    c.BaseAddress = new Uri("http://payment_service:8080");
                    c.Timeout = TimeSpan.FromSeconds(15);
                });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseExceptionHandler();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
