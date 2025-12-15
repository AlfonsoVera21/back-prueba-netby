using ProductService.Application.Commands;
using ProductService.Domain.Entities;
using ProductService.Domain.Ports;

namespace ProductService.Application.Handlers;

public class CreateProductHandler
{
    private readonly IProductRepository _repository;

    public CreateProductHandler(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> HandleAsync(CreateProductCommand command)
    {
        var product = new Product(
            command.Name,
            command.Description,
            command.Category,
            command.ImageUrl,
            command.Price,
            command.InitialStock);

        await _repository.AddAsync(product);
        return product.Id;
    }
}
