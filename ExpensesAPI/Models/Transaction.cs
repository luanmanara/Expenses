using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ExpensesAPI.Models
{
    public class Transaction
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("Period")]
        public int PeriodId { get; set; }
        public Period Period { get; set; }
        [Required]
        public double Value { get; set; }
        [Required, RegularExpression(@"1|2|3")]
        // 1 Credito - 2 Debito - 3 Salario
        public int TransactionType { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public DateTime DateOfMovement { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
