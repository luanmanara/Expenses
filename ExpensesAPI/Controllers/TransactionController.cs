using AutoMapper;
using ExpensesAPI.Models;
using ExpensesAPI.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using System.Net;

namespace ExpensesAPI.Controllers
{
    [Route("api/transaction")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionRepository _dbTransaction;
        private readonly IPeriodRepository _dbPeriod;
        private readonly IMapper _mapper;
        private APIResponse _response;

        public TransactionController(ITransactionRepository dbTransaction, IPeriodRepository dbPeriod, IMapper mapper)
        {
            _dbTransaction = dbTransaction;
            _dbPeriod = dbPeriod;
            _mapper = mapper;
            _response = new APIResponse();
        }

        [HttpGet]
        public async Task<ActionResult<APIResponse>> GetAllTransactions([FromQuery] int periodId)
        {
            try
            {
                List<Expression<Func<Transaction, bool>>> filters = new List<Expression<Func<Transaction, bool>>>();

                if (periodId > 0)
                {
                    Expression<Func<Transaction, bool>> filterPeriodId = x => x.PeriodId == periodId;
                    filters.Add(filterPeriodId);
                }

                List<TransactionDTO> transactionDTOList = _mapper.Map<List<TransactionDTO>>(await _dbTransaction.GetAllAsync(filters));
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = transactionDTOList;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        [HttpGet("{id}", Name = "GetTransaction")]
        public async Task<ActionResult<APIResponse>> GetTransaction(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var transaction = _mapper.Map<TransactionDTO>(await _dbTransaction.GetAsync(u => u.Id == id));
                if (transaction == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = transaction;

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
        public async Task<ActionResult<APIResponse>> CreateTransaction([FromBody] TransactionCreateDTO transactionCreateDTO)
        {
            try
            {
                if (transactionCreateDTO == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var period = await _dbPeriod.GetAsync(v => v.Id == transactionCreateDTO.PeriodId);
                if (period == null)
                {
                    ModelState.AddModelError("ErrorMessages", $"Period Id does not exist: {transactionCreateDTO.PeriodId}");
                    return BadRequest(ModelState);
                }

                if(period.IsClosed) 
                {
                    ModelState.AddModelError("ErrorMessages", $"Period is closed, create transaction is not allowed!");
                    return BadRequest(ModelState);
                }

                var transaction = _mapper.Map<Transaction>(transactionCreateDTO);
                await _dbTransaction.CreateAsync(transaction);

                _response.StatusCode = HttpStatusCode.Created;
                _response.Result = _mapper.Map<TransactionDTO>(transaction);

                return CreatedAtRoute("GetTransaction", new { transaction.Id }, _response);

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<APIResponse>> DeleteTransaction(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var transaction = await _dbTransaction.GetAsync(u => u.Id == id);

                if (transaction == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                await _dbTransaction.RemoveAsync(transaction);
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
        public async Task<ActionResult<APIResponse>> UpdateTransaction(int id, [FromBody] TransactionUpdateDTO transactionUpdateDTO)
        {
            try
            {
                if (id == 0 || id != transactionUpdateDTO.Id || transactionUpdateDTO == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var transaction = await _dbTransaction.GetAsync(u => u.Id == id, tracked: false);
                if (transaction == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                double difference = transactionUpdateDTO.Value != transaction.Value ?
                                    transaction.Value - transactionUpdateDTO.Value :
                                    0;

                var transactionMapped = _mapper.Map<TransactionUpdateDTO, Transaction>(transactionUpdateDTO, transaction);
                await _dbTransaction.UpdateAsync(transactionMapped, difference);

                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = _mapper.Map<TransactionDTO>(transactionMapped);

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
