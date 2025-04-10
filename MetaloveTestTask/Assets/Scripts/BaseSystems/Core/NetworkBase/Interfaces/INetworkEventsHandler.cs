using Scripts.BaseSystems.MessageProcessor;
using System;
using System.Collections.Generic;

namespace Scripts.BaseSystems.NetworkBase
{
    public interface INetworkEventsHandler
    {
        public event Action<int[], float?> OnInitializeNetwork;
        public event Func<bool> OnIsConnected;
        public event Action<float> OnSetWaitingConnectionSeconds;

        //  ----------------------------------------    Messages
        public event Action<IMessage> OnSendNetworkMessage;

        //  ----------------------------------------    Regions
        public event Action<int> OnConnectToRegion;
        public event Func<int> OnGetCurrentRegionId;
        public event Func<int, string> OnGetRegionNameById;
        public event Func<string, int> OnGetRegionIdByName;
        public event Func<INetworkRoomBs[]> OnGetRoomCollection;
        public event Func<int, string> OnGetLocalizedRegionNameById;
        public event Func<string, int?> OnGetRegionIdByLocalizedName;

        //  ----------------------------------------    Lobby
        public event Func<bool> OnIsInLobby;
        public event Action<int> OnJoinLobby;
        public event Func<int> OnGetCurrentLobbyId;

        //  ----------------------------------------    Room
        public event Action<INetworkRoomBs> OnCreateNewRoom;
        public event Action OnLeaveRoom;
        public event Action<string> OnJoinRoom;
        public event Func<bool> OnIsInsideTheRoom;
        public event Func<INetworkPlayerBs[]> OnGetPlayersInsideCurrentRoom;
        public event Action<int> OnKickPlayerFromRoom;
        public event Action<INetworkRoomBs> OnCopyPropertiesInToTheCurrentRoom;
        public event Func<INetworkRoomBs> OnGetCurrentRoomData;

        //  ----------------------------------------    Ping
        public event Func<int> OnGetCurrentRegionPing;
        public event Func<Dictionary<int, int>> OnGetRegionsPingCollection;

        //  ----------------------------------------    Player
        public event Func<bool> OnIsMasterClient;
        public event Action<INetworkPlayerBs> OnCopyInToTheLocalPlayerData;
        public event Func<INetworkPlayerBs> OnGetLocalPlayerData;
        public event Func<INetworkPlayerBs> OnGetMasterPlayerData;

        //  ----------------------------------------    Previous connection data
    }
}
