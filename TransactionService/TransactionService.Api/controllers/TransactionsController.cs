using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TransactionService.Application.Commands;
using TransactionService.Application.Handlers;
using TransactionService.Domain.Entities;
using TransactionService.Domain.Ports;

namespace TransactionService.Api.controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly RegisterTransactionHandler _registerHandler;
        private readonly ITransactionRepository _transactionRepository;

        public TransactionsController(
            RegisterTransactionHandler registerHandler,
            ITransactionRepository transactionRepository)
        {
            _registerHandler = registerHandler;
            _transactionRepository = transactionRepository;
        }

        // POST api/transactions
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterTransactionCommand cmd)
        {
            try
            {
                var id = await _registerHandler.HandleAsync(cmd);
                return CreatedAtAction(nameof(GetHistory),
                    new { productId = cmd.ProductId }, new { id });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // GET api/transactions  (Read - lista)
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _transactionRepository.GetAllAsync();
            return Ok(items);
        }

        // GET api/transactions/{id}  (Read - detalle)
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var tx = await _transactionRepository.GetByIdAsync(id);
            if (tx is null) return NotFound();
            return Ok(tx);
        }

        // PUT api/transactions/{id}  (Update)
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] RegisterTransactionCommand cmd)
        {
            var tx = await _transactionRepository.GetByIdAsync(id);
            if (tx is null) return NotFound();

            tx.Update(cmd.Type, cmd.Quantity, cmd.UnitPrice, cmd.Detail);

            await _transactionRepository.UpdateAsync(tx);

            return NoContent();
        }

        // DELETE api/transactions/{id}  (Delete)
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var tx = await _transactionRepository.GetByIdAsync(id);
            if (tx is null) return NotFound();

            await _transactionRepository.DeleteAsync(tx);

            return NoContent();
        }

        // GET api/transactions/history?productId=...&from=...&to=...&type=...
        [HttpGet("history")]
        public async Task<IActionResult> GetHistory(
            [FromQuery] Guid productId,
            [FromQuery] DateTime? from,
            [FromQuery] DateTime? to,
            [FromQuery] TransactionType? type)
        {
            var history = await _transactionRepository.GetHistoryAsync(productId, from, to, type);
            return Ok(history);
        }
    }
}
