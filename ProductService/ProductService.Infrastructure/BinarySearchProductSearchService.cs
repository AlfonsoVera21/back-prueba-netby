using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Entities;
using ProductService.Domain.Ports;

namespace ProductService.Infrastructure;

/// <summary>
/// Servicio de búsqueda en memoria usando lista ordenada y búsqueda binaria.
/// Pensado para escenarios con muchas consultas de lectura.
/// </summary>
public class BinarySearchProductSearchService : IProductSearchService
{
    private readonly ProductDbContext _context;
    private List<Product> _sortedProducts = new();
    private DateTime _lastLoad = DateTime.MinValue;
    private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(5);
    private readonly object _lock = new();

    public BinarySearchProductSearchService(ProductDbContext context)
    {
        _context = context;
    }

    private async Task EnsureCacheAsync()
    {
        if ((DateTime.UtcNow - _lastLoad) < _cacheDuration && _sortedProducts.Count > 0)
            return;

        var products = await _context.Products
            .AsNoTracking()
            .OrderBy(p => p.Name)
            .ToListAsync();

        lock (_lock)
        {
            _sortedProducts = products;
            _lastLoad = DateTime.UtcNow;
        }
    }

    public async Task<IReadOnlyList<Product>> SearchByNameAsync(string nameFragment)
    {
        await EnsureCacheAsync();

        if (string.IsNullOrWhiteSpace(nameFragment))
            return _sortedProducts;

        var normalized = nameFragment.Trim().ToUpperInvariant();

        List<Product> snapshot;
        lock (_lock)
        {
            snapshot = _sortedProducts;
        }

        int first = LowerBound(snapshot, normalized);
        int last = UpperBound(snapshot, normalized);

        if (first == -1 || last == -1 || last < first)
            return Array.Empty<Product>();

        return snapshot.Skip(first).Take(last - first + 1).ToList();
    }

    private static int LowerBound(List<Product> products, string fragment)
    {
        int left = 0, right = products.Count - 1;
        int ans = -1;

        while (left <= right)
        {
            int mid = (left + right) / 2;
            var midName = products[mid].Name.ToUpperInvariant();

            if (string.Compare(midName, fragment, StringComparison.Ordinal) >= 0)
            {
                if (midName.StartsWith(fragment, StringComparison.Ordinal))
                    ans = mid;
                right = mid - 1;
            }
            else
            {
                left = mid + 1;
            }
        }

        return ans;
    }

    private static int UpperBound(List<Product> products, string fragment)
    {
        int left = 0, right = products.Count - 1;
        int ans = -1;
        var limit = fragment + char.MaxValue;

        while (left <= right)
        {
            int mid = (left + right) / 2;
            var midName = products[mid].Name.ToUpperInvariant();

            if (string.Compare(midName, limit, StringComparison.Ordinal) <= 0)
            {
                if (midName.StartsWith(fragment, StringComparison.Ordinal))
                    ans = mid;
                left = mid + 1;
            }
            else
            {
                right = mid - 1;
            }
        }

        return ans;
    }
}
