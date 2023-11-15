using ExpensesAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;

namespace ExpensesAPI.Repository.IRepository
{
    public interface IUserRepository
    {
        bool isUniqueUser(string username);
        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);
        Task<UserDTO> Register(RegistrationRequestDTO registrationRequestDTO);
    }
}
