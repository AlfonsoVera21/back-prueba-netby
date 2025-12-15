namespace ProductService.Application.Commands;

public record UpdateProductCommand(
    Guid Id,
    string Name,
    string Description,
    string Category,
    string ImageUrl,
    decimal Price);
