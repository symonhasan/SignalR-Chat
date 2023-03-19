using SignalR_Chat.Models;

namespace SignalR_Chat.Utilities
{
    public class ConnectionManager : IConnectionManager
    {
        #region Fields

        public IList<ClientInfo> ClientInfos { get; set; }
        public IDictionary<string, string> ConnectionMap { get; set; }
        public List<string> AvailableClients { get; set; }

        #endregion

        #region Ctor

        public ConnectionManager()
        {
            ClientInfos = new List<ClientInfo>();
            ConnectionMap = new Dictionary<string, string>();
            AvailableClients = new List<string>();
        }

        #endregion

        #region Methods

        public void AddClientToList(ClientInfo clientInfo)
        {
            ClientInfos.Add(clientInfo);
            AvailableClients.Add(clientInfo.UserId);
        }

        public void RemoveClientFromList(ClientInfo clientInfo)
        {
            ClientInfos.Remove(clientInfo);
            AvailableClients.Remove(clientInfo.UserId);
        }

        public bool SearchForAvailableConnection(string userId)
        {
            foreach (var availableUserId in AvailableClients)
            {
                if (availableUserId == userId)
                {
                    continue;
                }
                var isAvailable = ConnectionMap.Any(x => x.Key == availableUserId);

                if (!isAvailable)
                {
                    ConnectionMap[userId] = availableUserId;
                    ConnectionMap[availableUserId] = userId;
                    return true;
                }
            }

            return false;
        }

        public void SkipCurrentConnection(string userId)
        {
            var connectedWith = ConnectionMap[userId];
            ConnectionMap.Remove(connectedWith);
            ConnectionMap.Remove(userId);
            AvailableClients.Add(connectedWith);
            AvailableClients.Add(userId);
        }

        public ClientInfo GetClientInfo(string userId)
        {
            return ClientInfos?.SingleOrDefault(x => x?.UserId == userId);
        }

        public ClientInfo GetConnectedWith(string userId)
        {
            return ClientInfos.SingleOrDefault(x => x.UserId == ConnectionMap[userId]);
        }

        #endregion
    }
}
