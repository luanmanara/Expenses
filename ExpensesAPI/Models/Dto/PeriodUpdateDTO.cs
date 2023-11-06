using System.ComponentModel.DataAnnotations;

namespace ExpensesAPI.Models
{
    public class PeriodUpdateDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public bool IsClosed { get; set; }
    }
}