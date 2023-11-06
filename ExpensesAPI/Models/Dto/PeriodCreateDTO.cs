using System.ComponentModel.DataAnnotations;

namespace ExpensesAPI.Models
{
    public class PeriodCreateDTO
    {
        [Required]
        public int WalletId { get; set; }
        [Required]
        public DateTime Month { get; set; }
    }
}