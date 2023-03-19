using SignalR_Chat.Models;

namespace SignalR_Chat.Utilities
{
    public interface IConnectionManager
    {
        void AddClientToList(ClientInfo clientInfo);
        void RemoveClientFromList(ClientInfo clientInfo);
        bool SearchForAvailableConnection(string userId);
        void SkipCurrentConnection(string userId);
        ClientInfo GetClientInfo(string userId);
        ClientInfo GetConnectedWith(string userId);
        void DisconnectClient(string connectionId);
        ClientInfo GetConnectedWithByConnectionId(string connectionId);
    }
}