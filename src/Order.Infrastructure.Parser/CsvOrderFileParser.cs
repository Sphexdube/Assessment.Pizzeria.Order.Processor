using Order.Domain.Observability.Interfaces;
using Order.Infrastructure.Parser.Interfaces;

namespace Order.Infrastructure.Parser
{
    public sealed class CsvOrderFileParser : IOrderFileParser
    {
        private readonly ILogger _logger;

        public CsvOrderFileParser(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<List<Domain.Entities.Order>> ParseOrderFileAsync(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    _logger.LogError($"File not found: {filePath}");
                    throw new FileNotFoundException($"Order file not found: {filePath}");
                }

                var entries = new List<Domain.Entities.Order>();
                var lines = await File.ReadAllLinesAsync(filePath);

                // Skip header row if it exists
                bool isFirstLine = true;
                foreach (var line in lines)
                {
                    if (isFirstLine)
                    {
                        isFirstLine = false;
                        // If this is a header row, skip it
                        if (line.Contains("OrderId", StringComparison.OrdinalIgnoreCase))
                            continue;
                    }

                    var values = line.Split(',');
                    if (values.Length >= 6)
                    {
                        try
                        {
                            entries.Add(new Domain.Entities.Order
                            {
                                OrderId = values[0].Trim(),
                                ProductId = values[1].Trim(),
                                Quantity = int.Parse(values[2].Trim()),
                                DeliverAt = DateTime.Parse(values[3].Trim()),
                                CreatedAt = DateTime.Parse(values[4].Trim()),
                                CustomerAddress = values[5].Trim()
                            });
                        }
                        catch (FormatException ex)
                        {
                            _logger.LogWarning($"Error parsing line: {line}. {ex.Message}");
                            // Continue with next line instead of failing the entire file
                        }
                    }
                    else
                    {
                        _logger.LogWarning($"Invalid line format: {line}");
                    }
                }

                return entries;
            }
            catch (Exception ex) when (ex is not FileNotFoundException)
            {
                _logger.LogError($"Unexpected error reading file: {filePath}", ex);
                throw;
            }
        }
    }
}
