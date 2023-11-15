using AutoMapper;
using ExpensesAPI.Data;
using ExpensesAPI.Models;
using ExpensesAPI.Repository.IRepository;
using MagicVilla_VillaAPI.Models.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;

namespace ExpensesAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly string secretKey;

        public UserRepository(ApplicationDbContext db,
                              IMapper mapper,
                              IConfiguration configuration,
                              UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _mapper = mapper;
            secretKey = configuration.GetValue<string>("ApiSettings:SecretKey");
            _userManager = userManager;
        }

        public bool isUniqueUser(string username)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.UserName == username);
            return user == null;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            Expression<Func<ApplicationUser, bool>> filter = u => u.UserName.ToLower() == loginRequestDTO.UserName.ToLower();

            var user = await _db.ApplicationUsers.FirstOrDefaultAsync(filter);
            bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDTO.Password);

            if (user == null || !isValid)
            {
                return new LoginResponseDTO()
                {
                    Token = "",
                    User = null
                };
            }

            // if user was found then generate token
            var roles = await _userManager.GetRolesAsync(user);
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, roles.FirstOrDefault())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            LoginResponseDTO loginResponseDTO = new LoginResponseDTO()
            {
                Token = tokenHandler.WriteToken(token),
                User = _mapper.Map<UserDTO>(user),
                Role = roles.FirstOrDefault()
            };

            return loginResponseDTO;
        }

        public async Task<UserDTO> Register(RegistrationRequestDTO registrationRequestDTO)
        {
            ApplicationUser user = new ApplicationUser()
            {
                UserName = registrationRequestDTO.UserName,
                Email = registrationRequestDTO.UserName,
                NormalizedEmail = registrationRequestDTO.UserName.ToUpper(),
            };

            try
            {
                var result = await _userManager.CreateAsync(user, registrationRequestDTO.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "admin");
                    var userToReturn = _db.ApplicationUsers.FirstOrDefault(u => u.UserName.ToLower() == registrationRequestDTO.UserName.ToLower());

                    return _mapper.Map<UserDTO>(userToReturn);
                }
            }
            catch (Exception ex)
            {

            }

            return new UserDTO();
        }
    }
}
