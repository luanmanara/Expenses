using ExpensesAPI.Repository.IRepository;
using MagicVilla_VillaAPI.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ExpensesAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUserRepository _dbUser;
        private APIResponse _response;
        public UsersController(IUserRepository dbUser)
        {
            _dbUser = dbUser;
            _response = new APIResponse();
        }

        [HttpPost("login")]
        public async Task<ActionResult<APIResponse>> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            LoginResponseDTO loginResponseDTO = await _dbUser.Login(loginRequestDTO);
            if (loginResponseDTO.User == null) 
            {
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Username or password incorrect!");
                return Ok(_response);
            }

            _response.StatusCode = HttpStatusCode.OK;
            _response.Result = loginResponseDTO;
            return Ok(_response);
        }
        [HttpPost("register")]
        public async Task<ActionResult<APIResponse>> Register([FromBody] RegistrationRequestDTO registrationRequestDTO)
        {
            if(!_dbUser.isUniqueUser(registrationRequestDTO.UserName))
            {
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Username already exists!");
                return Ok(_response);
            }

            var registerResponseDTO = await _dbUser.Register(registrationRequestDTO);
            if (!registerResponseDTO.IdentityResult.Succeeded)
            {
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = false;
                foreach (var e in registerResponseDTO.IdentityResult.Errors)
                {
                    _response.ErrorMessages.Add(e.Description);
                }

                return Ok(_response);
            }

            _response.StatusCode = HttpStatusCode.Created;
            _response.Result = registerResponseDTO.User;
            return Ok(_response);
        }
    }
}
