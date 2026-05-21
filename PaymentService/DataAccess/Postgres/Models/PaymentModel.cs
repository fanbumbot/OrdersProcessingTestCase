namespace PaymentService.DataAccess.Postgres.Models
{
    /// <summary>
    /// Класс-хранилище для данных об оплате
    /// </summary>
    public class PaymentModel
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public DateTime Timestamp { get; set; }
        public decimal Price { get; set; }
        public bool Status { get; set; } = false;
    }
}
