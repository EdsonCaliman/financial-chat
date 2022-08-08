namespace FinancialChat.Stock
{
    public interface IStockService
    {
        Task GetStockQuote(string symbol, string rooom);
    }
}