using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ExpensesAPI.Models
{
    public class TransactionUpdateDTO
    {
        public int Id { get; set; }
        public double Value { get; set; }
        public string Description { get; set; }
        public DateTime DateOfMovement { get; set; }
    }
}
