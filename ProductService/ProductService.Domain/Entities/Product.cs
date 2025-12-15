namespace ProductService.Domain.Entities;

public class Product
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public string Category { get; private set; } = null!;
    public string ImageUrl { get; private set; } = null!;
    public decimal Price { get; private set; }
    public int Stock { get; private set; }

    private Product() { }

    public Product(string name, string description, string category,
                   string imageUrl, decimal price, int initialStock)
    {
        Id = Guid.NewGuid();
        Update(name, description, category, imageUrl, price);
        Stock = initialStock;
    }

    public void Update(string name, string description, string category,
                       string imageUrl, decimal price)
    {
        Name = name;
        Description = description;
        Category = category;
        ImageUrl = imageUrl;
        Price = price;
    }

    public void IncreaseStock(int quantity)
    {
        if (quantity < 0) throw new ArgumentOutOfRangeException(nameof(quantity));
        Stock += quantity;
    }

    public void DecreaseStock(int quantity)
    {
        if (quantity < 0) throw new ArgumentOutOfRangeException(nameof(quantity));
        if (quantity > Stock)
        {
            throw new InvalidOperationException("Stock insuficiente.");
        }
        Stock -= quantity;
    }
}
