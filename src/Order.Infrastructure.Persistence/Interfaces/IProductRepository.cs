using Order.Domain.Models;

namespace Order.Infrastructure.Persistence.Interfaces
{
    public interface IProductRepository
    {
        Task<List<Product>> GetProductsAsync();
        Task<Product?> GetProductByIdAsync(string productId);
    }
}
