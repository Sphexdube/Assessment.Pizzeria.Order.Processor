using Order.Domain.Models;
using Order.Domain.Observability.Interfaces;
using Order.Infrastructure.Persistence.Interfaces;
using System.Text.Json;

namespace Order.Infrastructure.Persistence.Repositories
{
    public sealed class ProductRepository : IProductRepository
    {
        private readonly ILogger _logger;
        private List<Product>? _products;

        public ProductRepository(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<List<Product>> GetProductsAsync()
        {
            if (_products != null && _products.Count > 0)
            {
                return _products;
            }

            try
            {
                // Load products
                string productJson = await File.ReadAllTextAsync("products.json");
                var productDtos = JsonSerializer.Deserialize<List<Product>>(productJson,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<Product>();

                // Load ingredients
                string ingredientJson = await File.ReadAllTextAsync("ingredients.json");
                var productIngredients = JsonSerializer.Deserialize<List<Domain.Entities.ProductIngredient>>(ingredientJson,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<Domain.Entities.ProductIngredient>();

                // Map products with their ingredients
                _products = productDtos.Select(dto => new Product
                {
                    ProductId = dto.ProductId,
                    ProductName = dto.ProductName,
                    Price = dto.Price,
                    Vat = dto.Vat,
                    Ingredients = GetIngredientsForProduct(dto.ProductId, productIngredients)
                }).ToList();

                return _products;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error loading products or ingredients: {ex.Message}", ex);
                throw new InvalidOperationException("Failed to load product data", ex);
            }
        }

        public async Task<Product?> GetProductByIdAsync(string productId)
        {
            var products = await GetProductsAsync();
            return products.FirstOrDefault(p => p.ProductId == productId);
        }

        private List<Ingredient> GetIngredientsForProduct(string productId, List<Domain.Entities.ProductIngredient> productIngredients)
        {
            var productIngredient = productIngredients.FirstOrDefault(pi => pi.ProductId == productId);
            if (productIngredient == null) return new List<Ingredient>();

            return productIngredient.Ingredients.Select(i => new Ingredient
            {
                Name = i.Name,
                Amount = i.Amount,
                Unit = i.Unit
            }).ToList();
        }
    }
}
