using Scripts.BaseSystems.MessageProcessor;
using System.Collections.Generic;

namespace Scripts.BaseSystems.NetworkBase
{
    /// <summary>
    ///     Methods that are invoked by local client
    /// </summary>
    public interface INetworkEventsInvoker
    {
        public void InitializeNetwork(int[] lobbyId, float? waitingConnectingSeconds=null);
        public bool IsConnected();
        public void SetWaitingConnectionSeconds(float seconds);

        //  ----------------------------------------    Messages
        public void SendNetworkMessage(IMessage message);

        //  ----------------------------------------    Regions
        public void ConnectToRegion(int regionId);
        public int? GetCurrentRegionId();
        public string GetRegionNameById(int regionId);
        public string GetLocalizedRegionNameById(int regionId);
        public int? GetRegionIdByName(string regionName);
        public int? GetRegionIdByLocalizedName(string regionName);
        public INetworkRoomBs[] GetRoomCollection();

        //  ----------------------------------------    Lobby
        public bool IsInLobby();
        public void JoinLobby(int lobbyId);
        public int? GetCurrentLobbyId();

        //  ----------------------------------------    Room
        public void CreateNewRoom(INetworkRoomBs newRoomData);
        public void LeaveRoom();
        public void JoinRoom(string roomId);
        public bool IsInsideTheRoom();
        public INetworkPlayerBs[] GetPlayersInsideCurrentRoom();
        public void KickPlayerFromRoom(int playerLocalId);
        public void CopyDataInToTheCurrentRoom(INetworkRoomBs roomDataToCopy);
        public INetworkRoomBs GetCurrentRoomData();

        //  ----------------------------------------    Ping
        public int? GetCurrentRegionPing();
        public Dictionary<int, int> GetRegionsPingCollection();

        //  ----------------------------------------    Player
        public bool? IsMasterClient();
        public void CopyInToTheLocalPlayerData(INetworkPlayerBs playerData);
        public INetworkPlayerBs GetLocalPlayerData();
        public INetworkPlayerBs GetMasterPlayerData();

        //  ----------------------------------------    Previous connection data
    }
}
