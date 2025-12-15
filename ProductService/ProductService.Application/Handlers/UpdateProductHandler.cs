using ProductService.Application.Commands;
using ProductService.Domain.Ports;

namespace ProductService.Application.Handlers;

public class UpdateProductHandler
{
    private readonly IProductRepository _repository;

    public UpdateProductHandler(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task HandleAsync(UpdateProductCommand command)
    {
        var product = await _repository.GetByIdAsync(command.Id)
            ?? throw new KeyNotFoundException("Producto no encontrado");

        product.Update(
            command.Name,
            command.Description,
            command.Category,
            command.ImageUrl,
            command.Price);

        await _repository.UpdateAsync(product);
    }
}
