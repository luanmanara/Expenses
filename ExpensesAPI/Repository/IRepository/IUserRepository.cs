using ExpensesAPI.Models;
using ExpensesAPI.Models.Dto;
using MagicVilla_VillaAPI.Models.Dto;
using Microsoft.AspNetCore.Identity;

namespace ExpensesAPI.Repository.IRepository
{
    public interface IUserRepository
    {
        bool isUniqueUser(string username);
        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);
        Task<RegisterResponseDTO> Register(RegistrationRequestDTO registrationRequestDTO);
    }
}
