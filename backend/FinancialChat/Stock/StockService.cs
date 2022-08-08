using FinancialChat.Model;
using FinancialChat.Parameters;
using FinancialChat.Publisher;
using System.Globalization;

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

        public async Task GetStockQuote(string symbol, string room)
        {
            try
            {
                var uri = _externalServicesParameter.StooqUrl + $"q/l/?s={symbol}&f=sd2t2ohlcv&h&e=csv";

                var httpClient = new HttpClient();

                string[]? lastLine = null;

                using (var stream = await httpClient.GetStreamAsync(uri))
                {
                    using (var reader = new StreamReader(stream))
                    {
                        while (!reader.EndOfStream)
                        {
                            var line = reader.ReadLine();
                            lastLine = line.Split(',');
                        }
                    }
                }

                if (lastLine?.Length > 0)
                {
                    var stock = new StockQuote
                    {
                        Symbol = symbol,
                        Room = room,
                        Value = decimal.Parse(lastLine[6], CultureInfo.InvariantCulture)
                    };

                    _publisher.SendMessage(stock);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}