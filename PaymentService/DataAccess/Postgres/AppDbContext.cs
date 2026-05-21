using Microsoft.EntityFrameworkCore;

using PaymentService.DataAccess.Postgres.Models;

namespace PaymentService.DataAccess.Postgres
{
    /// <summary>
    /// Контекст базы данных для данных об оплате
    /// </summary>
    public class AppDbContext : DbContext
    {
        public DbSet<PaymentModel> Payments { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}
