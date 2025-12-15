namespace ProductService.Application.Commands;

public record CreateProductCommand(
    string Name,
    string Description,
    string Category,
    string ImageUrl,
    decimal Price,
    int InitialStock);
