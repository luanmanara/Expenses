using AutoMapper;
using ExpensesAPI.Models;
using ExpensesAPI.Models.Dto;
using ExpensesAPI.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ExpensesAPI.Controllers
{
    [Route("api/wallet")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly IWalletRepository _dbWallet;
        private readonly IMapper _mapper;
        private APIResponse _response;

        public WalletController(IWalletRepository dbWallet, IMapper mapper)
        {
            _dbWallet = dbWallet;
            _mapper = mapper;
            _response = new APIResponse();
        }

        [HttpGet]
        public async Task<ActionResult<APIResponse>> GetAllWallets()
        {
            try
            {
                List<WalletDTO> walletDTOList = _mapper.Map<List<WalletDTO>>(await _dbWallet.GetAllAsync());
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = walletDTOList;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        [HttpGet("{id}", Name = "GetWallet")]
        public async Task<ActionResult<APIResponse>> GetWallet(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var wallet = _mapper.Map<WalletDTO>(await _dbWallet.GetAsync(u => u.Id == id));
                if (wallet == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = wallet;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        [HttpPost]
        public async Task<ActionResult<APIResponse>> CreateWallet([FromBody] WalletCreateDTO walletCreateDTO)
        {
            try
            {
                if (walletCreateDTO == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var wallet = _mapper.Map<Wallet>(walletCreateDTO);
                await _dbWallet.CreateAsync(wallet);

                _response.StatusCode = HttpStatusCode.Created;
                _response.Result = _mapper.Map<WalletDTO>(wallet);

                return CreatedAtRoute("GetWallet", new { wallet.Id }, _response);

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<APIResponse>> DeleteWallet(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var wallet = await _dbWallet.GetAsync(u => u.Id == id);

                if (wallet == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                await _dbWallet.RemoveAsync(wallet);
                _response.StatusCode = HttpStatusCode.NoContent;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<APIResponse>> UpdateWallet(int id, [FromBody] WalletUpdateDTO walletUpdateDTO)
        {
            try
            {
                if (id == 0 || id != walletUpdateDTO.Id || walletUpdateDTO == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var wallet = await _dbWallet.GetAsync(u => u.Id == id, tracked: false);
                if (wallet == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                var walletMapped = _mapper.Map<WalletUpdateDTO, Wallet>(walletUpdateDTO, wallet);
                await _dbWallet.UpdateAsync(walletMapped);

                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = _mapper.Map<WalletDTO>(walletMapped);

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
        }
    }
}
