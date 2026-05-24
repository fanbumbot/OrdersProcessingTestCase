using NotificationService.Kafka;
using NotificationService.Notification;
using NotificationService.Notification.WebSocketHubs;

namespace NotificationService.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddSignalR();

            builder.Services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
            });

            builder.Services.AddSingleton<NotificationMapper>();
            builder.Services.AddHostedService<KafkaInitializer>();
            builder.Services.AddHostedService<KafkaConsumer>();

            var app = builder.Build();

            app.UseAuthorization();

            app.MapHub<NotificationHub>("/notifications");

            app.Run();
        }
    }
}
