using Scripts.BaseSystems.MessageProcessor;
using System.Collections.Generic;

namespace Scripts.BaseSystems.NetworkBase
{
    /// <summary>
    ///     Methods that are used by Network API to invoke events
    /// </summary>
    public interface INetworkCallbackInvoker
    {
        public void ConnectionOccured();
        public void ConnectionLost();

        //  ----------------------------------------    Messages
        public void ReceivedNetworkMessage(IMessage message);

        //  ----------------------------------------    Regions
        public void ConnectionToRegionOccured(int regionId);
        public void ConnectionToRegionFailed(int regionId);
        public void RoomCollectionUpdated(INetworkRoomBs[] roomCollection);

        //  ----------------------------------------    Lobby
        public void JoinLobbyOccured(int lobbyId);
        public void JoinLobbyFailed(int lobbyId);

        //  ----------------------------------------    Room
        public void RoomCreationOccured(INetworkRoomBs roomData);
        public void RoomCreationFailed();
        public void LeaveRoomOccured();
        public void JoinRoomOccured(INetworkRoomBs roomData);
        public void JoinRoomFailed(string name);
        public void AnotherPlayerJoinedTheRoom(INetworkPlayerBs playerData);
        public void AnotherPlayerLeftTheRoom(INetworkPlayerBs playerData);
        public void MasterClientSwitched(INetworkPlayerBs newMasterPlayerData);
        public void RoomPropertiesUpdated(INetworkRoomBs roomData);

        //  ----------------------------------------    Ping
        public void PingDataUpdated(Dictionary<int,int> pingDataCollection);

        //  ----------------------------------------    Player
        public void PlayerDataUpdated(INetworkPlayerBs playerData);
        public string GetLocalPlayerLogInName();

        //  ----------------------------------------    Previous connection data
        public int? GetPreviousConnectedLobbyId();
        public int? GetPreviousConnectedRegionId();
    }
}
