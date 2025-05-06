namespace Order.Application.Models.Interfaces
{
    public interface IOrderFileParser
    {
        Task<List<OrderFileEntry>> ParseOrderFileAsync(string filePath);
    }
}
