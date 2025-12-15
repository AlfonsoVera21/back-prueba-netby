using Microsoft.EntityFrameworkCore;
using TransactionService.Domain.Entities;
using TransactionService.Domain.Ports;

namespace TransactionService.Infrastructure;

public class EfTransactionRepository : ITransactionRepository
{
    private readonly TransactionDbContext _context;

    public EfTransactionRepository(TransactionDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(StockTransaction transaction)
    {
        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync();
    }

    public async Task<IReadOnlyList<StockTransaction>> GetHistoryAsync(Guid productId, DateTime? from, DateTime? to, TransactionType? type)
    {
        var query = _context.Transactions.AsNoTracking().Where(t => t.ProductId == productId);

        if (from.HasValue)
            query = query.Where(t => t.Date >= from.Value);
        if (to.HasValue)
            query = query.Where(t => t.Date <= to.Value);
        if (type.HasValue)
            query = query.Where(t => t.Type == type.Value);

        return await query.OrderByDescending(t => t.Date).ToListAsync();
    }
}
