using FinancialChat.Hubs;
using FinancialChat.Model;
using FinancialChat.Parameters;
using Microsoft.AspNetCore.SignalR.Client;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace FinancialChat.Consumer
{
    public class ConsumerService : BackgroundService
    {
        private readonly string _botUser;
        private readonly RabbitParameters _rabbitParameters;

        public ConsumerService(RabbitParameters rabbitParameters)
        {
            _botUser = "Bot";
            _rabbitParameters = rabbitParameters;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
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

            channel.QueueDeclare(queue: _rabbitParameters.Queue,
                                durable: false,
                                exclusive: false,
                                autoDelete: false,
                                arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += Consumer_Received;
            channel.BasicConsume(queue: _rabbitParameters.Queue,
                autoAck: true,
                consumer: consumer);

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }

        private async void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            var body = Encoding.UTF8.GetString(e.Body.ToArray());

            var message = JsonSerializer.Deserialize<StockQuote>(body);

            HubConnection connection = new HubConnectionBuilder()
                .WithUrl(new Uri("http://localhost:5187/chat"))
                .WithAutomaticReconnect()
                .Build();

            await connection.StartAsync();

            var user = new UserConnection
            {
                User = _botUser,
                Room = message.Room
            };
            await connection.InvokeAsync("JoinRoom", user);
            var messageStock = $"{message.Symbol} quote is ${message.Value} per share";
            await connection.InvokeAsync("SendMessage", messageStock);
        }
    }
}