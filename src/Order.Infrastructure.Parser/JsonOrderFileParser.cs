using Order.Application.Models;
using Order.Application.Models.Interfaces;
using Order.Domain.Observability.Interfaces;
using System.Text.Json;

namespace Order.Infrastructure.Parser
{
    public class JsonOrderFileParser : IOrderFileParser
    {
        private readonly ILogger _logger;

        public JsonOrderFileParser(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<List<OrderFileEntry>> ParseOrderFileAsync(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    _logger.LogError($"File not found: {filePath}");
                    throw new FileNotFoundException($"Order file not found: {filePath}");
                }

                string jsonContent = await File.ReadAllTextAsync(filePath);
                var entries = JsonSerializer.Deserialize<List<OrderFileEntry>>(jsonContent,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return entries ?? new List<OrderFileEntry>();
            }
            catch (JsonException ex)
            {
                _logger.LogError($"Error parsing JSON file: {filePath}", ex);
                throw new InvalidOperationException($"Error parsing order file: {ex.Message}", ex);
            }
            catch (Exception ex) when (ex is not FileNotFoundException)
            {
                _logger.LogError($"Unexpected error reading file: {filePath}", ex);
                throw;
            }
        }
    }
}
