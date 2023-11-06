namespace ExpensesAPI.Models
{
    public class PeriodDTO
    {
        public int Id { get; set; }
        public int WalletId { get; set; }
        public DateTime Month { get; set; }
        public double Salary { get; set; }
        public double Balance { get; set; }
        public bool IsClosed { get; set; }
    }
}