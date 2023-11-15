using ExpensesAPI.Models;
using ExpensesAPI.Repository.IRepository;
using MagicVilla_VillaAPI.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ExpensesAPI.Controllers
{
    [Route("/auth")]
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
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Username or password incorrect!");
                return BadRequest(_response);
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
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Username already exists!");
                return BadRequest(_response);
            }
            var registeredUser = await _dbUser.Register(registrationRequestDTO);
            if (registeredUser == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Error while registering!");
                return BadRequest(_response);
            }

            _response.StatusCode = HttpStatusCode.OK;
            _response.Result = registeredUser;
            return Ok(_response);
        }
    }
}
