namespace TransactionService.Domain.Entities;

public enum TransactionType
{
    Purchase = 1,
    Sale = 2
}

public class StockTransaction
{
    public Guid Id { get; private set; }
    public Guid ProductId { get; private set; }
    public DateTime Date { get; private set; }
    public TransactionType Type { get; private set; }
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public string Detail { get; private set; } = string.Empty;

    private StockTransaction() { }

    public StockTransaction(Guid productId, DateTime date, TransactionType type,
                            int quantity, decimal unitPrice, string detail)
    {
        Id = Guid.NewGuid();
        ProductId = productId;
        Date = date;
        Type = type;
        Quantity = quantity;
        UnitPrice = unitPrice;
        Detail = detail;
    }

    public void Update(TransactionType type, int quantity, decimal unitPrice, string detail)
    {
        Type = type;
        Quantity = quantity;
        UnitPrice = unitPrice;
        Detail = detail;
    }
}
