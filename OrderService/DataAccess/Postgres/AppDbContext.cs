using Microsoft.EntityFrameworkCore;

using OrderService.DataAccess.Postgres.Models;

namespace OrderService.DataAccess.Postgres
{
    /// <summary>
    /// Контекст о базе данных
    /// </summary>
    public class AppDbContext : DbContext
    {
        public DbSet<OrderModel> Orders { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}
