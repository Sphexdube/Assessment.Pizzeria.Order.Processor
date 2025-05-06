using Order.Application.Models;
using Order.Application.Models.Interfaces;
using Order.Domain.Models;
using Order.Domain.Observability.Interfaces;
using System.Text.Json;

namespace Order.Infrastructure.Parser
{
    public class ProductRepository : IProductRepository
    {
        private readonly string _productsFilePath;
        private readonly string _ingredientsFilePath;
        private readonly ILogger _logger;
        private List<Product>? _products;

        public ProductRepository(string productsFilePath, string ingredientsFilePath, ILogger logger)
        {
            _productsFilePath = productsFilePath;
            _ingredientsFilePath = ingredientsFilePath;
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
                string productJson = await File.ReadAllTextAsync(_productsFilePath);
                var productDtos = JsonSerializer.Deserialize<List<ProductDto>>(productJson,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<ProductDto>();

                // Load ingredients
                string ingredientJson = await File.ReadAllTextAsync(_ingredientsFilePath);
                var productIngredients = JsonSerializer.Deserialize<List<ProductIngredientDto>>(ingredientJson,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<ProductIngredientDto>();

                // Map products with their ingredients
                _products = productDtos.Select(dto => new Product
                {
                    ProductId = dto.ProductId,
                    ProductName = dto.ProductName,
                    Price = dto.Price,
                    VAT = dto.VAT,
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

        private List<Ingredient> GetIngredientsForProduct(string productId, List<ProductIngredientDto> productIngredients)
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
