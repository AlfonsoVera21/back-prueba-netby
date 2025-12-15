using TransactionService.Application.Commands;
using TransactionService.Domain.Entities;
using TransactionService.Domain.Ports;

namespace TransactionService.Application.Handlers;

public class RegisterTransactionHandler
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IProductStockPort _productStockPort;

    public RegisterTransactionHandler(ITransactionRepository transactionRepository, IProductStockPort productStockPort)
    {
        _transactionRepository = transactionRepository;
        _productStockPort = productStockPort;
    }

    public async Task<Guid> HandleAsync(RegisterTransactionCommand cmd)
    {
        if (cmd.Quantity <= 0)
            throw new ArgumentOutOfRangeException(nameof(cmd.Quantity), "La cantidad debe ser mayor a cero.");

        if (cmd.Type == TransactionType.Sale)
        {
            var currentStock = await _productStockPort.GetCurrentStockAsync(cmd.ProductId);
            if (cmd.Quantity > currentStock)
                throw new InvalidOperationException("Stock insuficiente para la venta.");
        }

        var tx = new StockTransaction(
            cmd.ProductId,
            DateTime.UtcNow,
            cmd.Type,
            cmd.Quantity,
            cmd.UnitPrice,
            cmd.Detail);

        await _transactionRepository.AddAsync(tx);

        if (cmd.Type == TransactionType.Purchase)
        {
            await _productStockPort.IncreaseStockAsync(cmd.ProductId, cmd.Quantity);
        }
        else
        {
            await _productStockPort.DecreaseStockAsync(cmd.ProductId, cmd.Quantity);
        }

        return tx.Id;
    }
}
