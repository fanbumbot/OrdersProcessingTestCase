using FluentValidation;
using MediatR;
using Refit;
using Microsoft.EntityFrameworkCore;
using OrderService.DataAccess.Postgres;
using OrderService.WebAPI.Client;
using OrderService.WebAPI.UseCases;

namespace OrderService.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

            builder.Services.AddControllers();

            builder.Services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            });

            builder.Services.AddExceptionHandler<ExceptionHandler>();
            builder.Services.AddProblemDetails();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

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
