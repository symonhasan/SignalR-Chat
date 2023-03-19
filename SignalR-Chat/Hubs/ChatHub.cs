using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalR_Chat.Models;
using SignalR_Chat.Utilities;

namespace SignalR_Chat.Hubs
{
    public class ChatHub : Hub
    {
        #region Fields

        private readonly IConnectionManager _connectionManager;

        #endregion

        #region Ctor

        public ChatHub(IConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
        }

        #endregion

        #region Methods

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var connectedWith = _connectionManager.GetConnectedWithByConnectionId(Context.ConnectionId);
            await Clients.Client(connectedWith.ConnectionId).SendAsync("SkipCurrentChat");
            _connectionManager.DisconnectClient(Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }

        public void Subscribe(string userId, string userName)
        {
            var connectionId = Context.ConnectionId;
            _connectionManager.AddClientToList(new ClientInfo
            {
                UserId = userId,
                UserName = userName,
                ConnectionId = connectionId
            });
        }

        public void SearchForConnection(string userId)
        {
            var isSuccessfull = _connectionManager.SearchForAvailableConnection(userId);
            
            if (isSuccessfull)
            {
                var currentUser = _connectionManager.GetClientInfo(userId);
                var connectedWith = _connectionManager.GetConnectedWith(userId);
                Clients.Client(currentUser.ConnectionId).SendAsync("ConnectionEstablished", $"You are connected with {connectedWith.UserName}.");
                Clients.Client(connectedWith.ConnectionId).SendAsync("ConnectionEstablished", $"You are connected with {currentUser.UserName}.");
            }
        }

        public async Task SendMessage(string userId, string message)
        {
            var currentUser = _connectionManager.GetClientInfo(userId);
            var connectedWith = _connectionManager.GetConnectedWith(userId);
            await Clients.Client(currentUser.ConnectionId).SendAsync("ReceiveMessage", message, userId);
            await Clients.Client(connectedWith.ConnectionId).SendAsync("ReceiveMessage", message, userId);
        }

        public async Task SkipCurrentChat(string userId)
        {
            var currentUser = _connectionManager.GetClientInfo(userId);
            var connectedWith = _connectionManager.GetConnectedWith(userId);
            _connectionManager.SkipCurrentConnection(userId);
            await Clients.Client(currentUser.ConnectionId).SendAsync("SkipCurrentChat");
            await Clients.Client(connectedWith.ConnectionId).SendAsync("SkipCurrentChat");
        }

        #endregion
    }
}
