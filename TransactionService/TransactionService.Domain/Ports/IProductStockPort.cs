namespace TransactionService.Domain.Ports;

public interface IProductStockPort
{
    Task<int> GetCurrentStockAsync(Guid productId);
    Task<bool> IncreaseStockAsync(Guid productId, int quantity);
    Task<bool> DecreaseStockAsync(Guid productId, int quantity);
}
