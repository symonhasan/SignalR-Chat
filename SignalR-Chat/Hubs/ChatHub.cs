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
                Clients.Client(currentUser.ConnectionId).SendAsync("ConnectionEstablished", $"Connected with {connectedWith.UserName}");
                Clients.Client(connectedWith.ConnectionId).SendAsync("ConnectionEstablished", $"Connected with {currentUser.UserName}");
            }
        }

        public async Task SendMessage(string userId, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", userId, message);
        }

        #endregion
    }
}
