using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Order.Application.Models.Interfaces;
using Order.Domain.Observability;
using Order.Domain.Observability.Interfaces;
using Order.Infrastructure.Parser;
using Order.Infrastructure.Parser.Interfaces;
using Order.Infrastructure.Persistence.Interfaces;
using Order.Infrastructure.Persistence.Repositories;

namespace Order.Presentation.Service.Configuration
{
    public static class HostBuilder
    {
        public static IHostBuilder CreateHostBuilder(string[] args) => Host
            .CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                services.AddSingleton<ILogger, ConsoleLogger>();
                services.AddSingleton<IOrderFileParser, JsonOrderFileParser>();
                services.AddSingleton<IProductRepository, ProductRepository>();
                services.AddSingleton<IOrderQueue, MockOrderQueue>();
                services.AddSingleton<OrderProcessingService>();
                services.AddSingleton<OrderPresentationService>();
            });
    }
}
