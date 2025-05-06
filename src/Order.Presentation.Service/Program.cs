using Microsoft.Extensions.DependencyInjection;
using Order.Presentation.Service.Configuration;

namespace Order.Presentation.Service
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var host = HostBuilder.CreateHostBuilder(args).Build();

            var orderService = host.Services.GetRequiredService<OrderPresentationService>();
            await orderService.RunAsync(args);
        }
    }
}