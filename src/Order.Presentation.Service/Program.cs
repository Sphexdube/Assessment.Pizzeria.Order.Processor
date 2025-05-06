using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Order.Application.Models.Interfaces;
using Order.Domain.Observability;
using Order.Domain.Observability.Interfaces;
using Order.Infrastructure.Parser;

namespace Order.Presentation.Service
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            var orderService = host.Services.GetRequiredService<OrderPresentationService>();
            await orderService.RunAsync(args);
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    services.AddSingleton<ILogger, ConsoleLogger>();
                    services.AddSingleton<IOrderFileParser, JsonOrderFileParser>(); // Default to JSON
                    services.AddSingleton<IProductRepository>(provider =>
                        new ProductRepository(
                            "products.json",
                            "ingredients.json",
                            provider.GetRequiredService<ILogger>()));
                    services.AddSingleton<IOrderQueue, MockOrderQueue>();
                    services.AddSingleton<OrderProcessingService>();
                    services.AddSingleton<OrderPresentationService>();
                });
    }
}