using FinancialChat.Data;
using FinancialChat.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace FinancialChat.Hubs
{
    public class ChatHub : Hub
    {
        private readonly string _botUser;
        private readonly IDictionary<string, UserConnection> _connections;
        private readonly AuthDbContext _context;

        public ChatHub(IDictionary<string, UserConnection> connections, AuthDbContext context)
        {
            _botUser = "Bot";
            _connections = connections;
            _context = context;
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

        [Authorize]
        public async Task JoinRoom(UserConnection userConnection)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userConnection.Room);

            await GetMessages(userConnection);

            _connections[Context.ConnectionId] = userConnection;

            await Clients.Group(userConnection.Room).SendAsync("ReceiveMessage", _botUser, $"{userConnection.User} has joined {userConnection.Room}");
        }

        [Authorize]
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

        [Authorize]
        public async Task SendMessage(string textMessage)
        {
            if (_connections.TryGetValue(Context.ConnectionId, out UserConnection userConnection))
            {
                var message = new Message()
                {
                    Text = textMessage,
                    Room = userConnection.Room,
                    User = userConnection.User
                };
                await _context.Messages.AddAsync(message);
                await _context.SaveChangesAsync();
                await Clients.Group(userConnection.Room).SendAsync("ReceiveMessage", userConnection.User, textMessage);
            }
        }
    }
}