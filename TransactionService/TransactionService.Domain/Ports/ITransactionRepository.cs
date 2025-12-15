using TransactionService.Domain.Entities;

namespace TransactionService.Domain.Ports;

public interface ITransactionRepository
{
    Task AddAsync(StockTransaction transaction);
    Task<StockTransaction?> GetByIdAsync(Guid id);
    Task<IReadOnlyList<StockTransaction>> GetAllAsync();
    Task UpdateAsync(StockTransaction transaction);
    Task DeleteAsync(StockTransaction transaction);
    Task<IReadOnlyList<StockTransaction>> GetHistoryAsync(Guid productId, DateTime? from, DateTime? to, TransactionType? type);
}
