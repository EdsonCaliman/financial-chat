using Microsoft.AspNetCore.SignalR;

namespace FinancialChat.Hubs
{
    public class ChatHub : Hub
    {
        private readonly string _botUser;

        public ChatHub()
        {
            _botUser = "Chat Bot";
        }

        public async Task JoinRoom(UserConnection userConnection)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userConnection.Room);
            await Clients.Group(userConnection.Room).SendAsync("receiveMessage", _botUser,
                $"{userConnection.User} has joined {userConnection.Room}");
        }
    }
}