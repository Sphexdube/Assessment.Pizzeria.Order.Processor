namespace Order.Infrastructure.Parser.Interfaces
{
    public interface IOrderFileParser
    {
        Task<List<Domain.Entities.Order>> ParseOrderFileAsync(string filePath);
    }
}
