using ProductService.Domain.Entities;

namespace ProductService.Domain.Ports;

public interface IProductSearchService
{
    Task<IReadOnlyList<Product>> SearchByNameAsync(string nameFragment);
}
