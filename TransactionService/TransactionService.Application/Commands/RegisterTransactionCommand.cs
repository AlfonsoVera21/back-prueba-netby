using TransactionService.Domain.Entities;

namespace TransactionService.Application.Commands;

public record RegisterTransactionCommand(
    Guid ProductId,
    TransactionType Type,
    int Quantity,
    decimal UnitPrice,
    string Detail);
