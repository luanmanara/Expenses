using AutoMapper;
using ExpensesAPI.Models;
using ExpensesAPI.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using System.Net;

namespace ExpensesAPI.Controllers
{
    [Route("api/period")]
    [ApiController]
    [Authorize]
    public class PeriodController : ControllerBase
    {
        private readonly IPeriodRepository _dbPeriod;
        private readonly IMapper _mapper;
        private APIResponse _response;

        public PeriodController(IPeriodRepository dbPeriod, IMapper mapper)
        {
            _dbPeriod = dbPeriod;
            _mapper = mapper;
            _response = new APIResponse();
        }

        [HttpGet]
        public async Task<ActionResult<APIResponse>> GetAllPeriods([FromQuery] int walletId, [FromQuery] bool? isClosed)
        {
            try
            {
                //List<Expression<Func<Period, bool>>> filters = x => true;
                List<Expression<Func<Period, bool>>> filters = new List<Expression<Func<Period, bool>>>();

                if (walletId > 0)
                {
                    Expression<Func<Period, bool>> filterWalletId = x => x.WalletId == walletId;
                    filters.Add(filterWalletId);
                }

                if (isClosed != null)
                {
                    Expression<Func<Period, bool>> filterIsClosed = x => x.IsClosed == isClosed;
                    filters.Add(filterIsClosed);
                }

                List<PeriodDTO> PeriodDTOList = _mapper.Map<List<PeriodDTO>>(await _dbPeriod.GetAllAsync(filters));
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = PeriodDTOList;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        [HttpGet("{id}", Name = "GetPeriod")]
        public async Task<ActionResult<APIResponse>> GetPeriod(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var period = _mapper.Map<PeriodDTO>(await _dbPeriod.GetAsync(u => u.Id == id));
                if (period == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = period;

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
        public async Task<ActionResult<APIResponse>> CreatePeriod([FromBody] PeriodCreateDTO PeriodCreateDTO)
        {
            try
            {
                if (PeriodCreateDTO == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var period = _mapper.Map<Period>(PeriodCreateDTO);
                await _dbPeriod.CreateAsync(period);

                _response.StatusCode = HttpStatusCode.Created;
                _response.Result = _mapper.Map<PeriodDTO>(period);

                return CreatedAtRoute("GetPeriod", new { period.Id }, _response);

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<APIResponse>> DeletePeriod(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var period = await _dbPeriod.GetAsync(u => u.Id == id);

                if (period == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                await _dbPeriod.RemoveAsync(period);
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
        public async Task<ActionResult<APIResponse>> UpdatePeriod(int id, [FromBody] PeriodUpdateDTO periodUpdateDTO)
        {
            try
            {
                if (id == 0 || id != periodUpdateDTO.Id || periodUpdateDTO == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var period = await _dbPeriod.GetAsync(u => u.Id == id, tracked: false);
                if (period == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                if (period.IsClosed == periodUpdateDTO.IsClosed)
                {
                    string errorMessage = "This period is already " + (period.IsClosed ? "closed" : "opened");
                    ModelState.AddModelError("ErrorMessages", errorMessage);
                    return BadRequest(ModelState);
                }

                var periodMapped = _mapper.Map<PeriodUpdateDTO, Period>(periodUpdateDTO, period);
                await _dbPeriod.CloseAsync(periodMapped);

                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = _mapper.Map<PeriodDTO>(periodMapped);

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
