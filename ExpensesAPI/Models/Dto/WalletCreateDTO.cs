using System.ComponentModel.DataAnnotations;

namespace ExpensesAPI.Models.Dto
{
    public class WalletCreateDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public double Saved { get; set; }
        public string UserId { get; set; }
    }
}
