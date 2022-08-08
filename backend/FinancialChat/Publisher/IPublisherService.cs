using FinancialChat.Model;

namespace FinancialChat.Publisher
{
    public interface IPublisherService
    {
        void SendMessage(StockQuote stockQuote);
    }
}
