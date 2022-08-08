using FinancialChat.Model;
using FinancialChat.Parameters;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace FinancialChat.Publisher
{
    public class PublisherService : IPublisherService
    {
        private readonly RabbitParameters _rabbitParameters;

        public PublisherService(RabbitParameters rabbitParameters)
        {
            _rabbitParameters = rabbitParameters;
        }

        public void SendMessage(StockQuote stockQuote)
        {
            var factory = new ConnectionFactory()
            {
                HostName = _rabbitParameters.HostName,
                UserName = _rabbitParameters.UserName,
                Password = _rabbitParameters.Password,
                Port = _rabbitParameters.Port
            };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(
                queue: _rabbitParameters.Queue,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            string jsonString = JsonSerializer.Serialize(stockQuote);
            var body = Encoding.UTF8.GetBytes(jsonString);

            channel.BasicPublish(exchange: string.Empty,
                                 routingKey: _rabbitParameters.Queue,
                                 basicProperties: null,
                                 body: body);
        }
    }
}
