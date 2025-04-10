using Scripts.BaseSystems.MessageProcessor;
using System;
using System.Collections.Generic;

namespace Scripts.BaseSystems.NetworkBase
{
    /// <summary>
    ///     Events emited by network API
    /// </summary>
    public interface INetworkCallbackHandler
    {
        public event Action OnConnectionOccured;
        public event Action OnConnectionLost;

        //  ----------------------------------------    Messages
        public event Action<IMessage> OnReceivedNetworkMessage;

        //  ----------------------------------------    Regions
        public event Action<int> OnConnectionToRegionOccured;
        public event Action<int> OnConnectionToRegionFailed;
        public event Action<INetworkRoomBs[]> OnRoomCollectionUpdated;

        //  ----------------------------------------    Lobby
        public event Action<int> OnJoinLobbyOccured;
        public event Action<int> OnJoinLobbyFailed;

        //  ----------------------------------------    Room
        public event Action<INetworkRoomBs> OnRoomCreationOccured;
        public event Action OnRoomCreationFailed;
        public event Action OnLeaveRoomOccured;
        public event Action<INetworkRoomBs> OnJoinRoomOccured;
        public event Action<string> OnJoinRoomFailed;
        public event Action<INetworkPlayerBs> OnAnotherPlayerJoinedTheRoom;
        public event Action<INetworkPlayerBs> OnAnotherPlayerLeftTheRoom;
        public event Action<INetworkPlayerBs> OnMasterClientSwitched;
        public event Action<INetworkRoomBs> OnRoomPropertiesUpdated;

        //  ----------------------------------------    Ping
        public event Action<Dictionary<int, int>> OnPingDataUpdated;

        //  ----------------------------------------    Player
        public event Action<INetworkPlayerBs> OnPlayerDataUpdated;
        public event Func<string> OnGetLocalPlayerLogInName;

        //  ----------------------------------------    Previous connection data
        public event Func<int?> OnGetPreviousConnectedLobbyId;
        public event Func<int?> OnGetPreviousConnectedRegionId;
    }
}
