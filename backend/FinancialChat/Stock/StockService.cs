using FinancialChat.Model;
using FinancialChat.Parameters;
using FinancialChat.Publisher;

namespace FinancialChat.Stock
{
    public class StockService : IStockService
    {
        private readonly IPublisherService _publisher;
        private readonly ExternalServicesParameter _externalServicesParameter;

        public StockService(IPublisherService publisher, ExternalServicesParameter externalServicesParameter)
        {
            _publisher = publisher;
            _externalServicesParameter = externalServicesParameter;
        }

        public void GetStockQuote(string symbol)
        {
            //https://stooq.com/q/l/?s=aapl.us&f=sd2t2ohlcv&h&e=csv


            var stock = new StockQuote { Symbol = symbol, Value = 10 };

            _publisher.SendMessage(stock);
        }
    }
}
