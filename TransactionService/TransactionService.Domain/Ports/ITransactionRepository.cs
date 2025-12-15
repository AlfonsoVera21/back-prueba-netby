using TransactionService.Domain.Entities;

namespace TransactionService.Domain.Ports;

public interface ITransactionRepository
{
    Task AddAsync(StockTransaction transaction);
    Task<IReadOnlyList<StockTransaction>> GetHistoryAsync(Guid productId, DateTime? from, DateTime? to, TransactionType? type);
}
