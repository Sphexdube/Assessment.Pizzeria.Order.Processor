namespace Order.Application.Models.Interfaces
{
    public interface IOrderQueue
    {
        Task EnqueueOrderAsync(Domain.Models.Order order);
    }
}
