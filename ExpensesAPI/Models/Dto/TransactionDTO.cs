using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ExpensesAPI.Models
{
    public class TransactionDTO
    {
        public int Id { get; set; }
        public int PeriodId { get; set; }
        public double Value { get; set; }
        public int TransactionType { get; set; }
        public string Description { get; set; }
        public DateTime DateOfMovement { get; set; }
    }
}
