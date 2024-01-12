using MagicVilla_VillaAPI.Models.Dto;
using Microsoft.AspNetCore.Identity;

namespace ExpensesAPI.Models.Dto
{
    public class RegisterResponseDTO
    {
        public IdentityResult IdentityResult { get; set; }
        public UserDTO User { get; set; }
    }
}
