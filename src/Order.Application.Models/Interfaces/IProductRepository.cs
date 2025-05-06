using Order.Domain.Models;

namespace Order.Application.Models.Interfaces
{
    public interface IProductRepository
    {
        Task<List<Product>> GetProductsAsync();
        Task<Product?> GetProductByIdAsync(string productId);
    }
}
