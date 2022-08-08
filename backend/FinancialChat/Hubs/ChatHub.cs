using FinancialChat.Data;
using FinancialChat.Model;
using FinancialChat.Stock;
using Microsoft.AspNetCore.SignalR;

namespace FinancialChat.Hubs
{
    public class ChatHub : Hub
    {
        private readonly string _botUser;
        private readonly IDictionary<string, UserConnection> _connections;
        private readonly AuthDbContext _context;
        private readonly IStockService _stockService;

        public ChatHub(IDictionary<string, UserConnection> connections, AuthDbContext context, IStockService stockService)
        {
            _botUser = "Bot";
            _connections = connections;
            _context = context;
            _stockService = stockService;
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            if (_connections.TryGetValue(Context.ConnectionId, out UserConnection userConnection))
            {
                _connections.Remove(Context.ConnectionId);
                Clients.Group(userConnection.Room).SendAsync("ReceiveMessage", _botUser, $"{userConnection.User} has left");
            }

            return base.OnDisconnectedAsync(exception);
        }

        public async Task JoinRoom(UserConnection userConnection)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userConnection.Room);

            _connections[Context.ConnectionId] = userConnection;

            if (userConnection.User == _botUser)
            {
                return;
            }

            await GetMessages(userConnection);

            await Clients.Group(userConnection.Room).SendAsync("ReceiveMessage", _botUser, $"{userConnection.User} has joined {userConnection.Room}");
        }

        private async Task GetMessages(UserConnection userConnection)
        {
            var messages = _context.Messages
                .Where(x => x.Room == userConnection.Room)
                .OrderBy(x => x.CreatedAt)
                .Take(50);

            foreach (var message in messages)
            {
                await Clients.Group(userConnection.Room).SendAsync("ReceiveMessage", userConnection.User, message.Text);
            }
        }

        public async Task SendMessage(string messageReceived)
        {
            if (_connections.TryGetValue(Context.ConnectionId, out UserConnection userConnection))
            {
                if (messageReceived.StartsWith("/stock"))
                {
                    String[] result = messageReceived.Split('=');
                    var symbol = result[1];
                    await _stockService.GetStockQuote(symbol, userConnection.Room);
                    return;
                }

                await SaveMessageToDatabase(messageReceived, userConnection);

                await Clients.Group(userConnection.Room).SendAsync("ReceiveMessage", userConnection.User, messageReceived);
            }
        }

        private async Task SaveMessageToDatabase(string messageReceived, UserConnection userConnection)
        {
            var message = new Message
            {
                Text = messageReceived,
                Room = userConnection.Room,
                User = userConnection.User,
            };

            await _context.Messages.AddAsync(message);
            await _context.SaveChangesAsync();
        }
    }
}