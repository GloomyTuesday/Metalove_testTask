using Scripts.BaseSystems.MessageProcessor;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.BaseSystems.NetworkBase
{
    [CreateAssetMenu ( menuName= "Scriptable Obj/Base systems/Core/Network base/Network api events" ) ]
    public class NetworkApiEventsSrc : ScriptableObject,
        INetworkCallbackHandler,
        INetworkCallbackInvoker,
        INetworkEventsInvoker,
        INetworkEventsHandler
    {
        //  ----------------------------------------    Network api events callback related
        #region Network api events callback related

        private Action _onConnectionOccured;
        event Action INetworkCallbackHandler.OnConnectionOccured
        {
            add => _onConnectionOccured += value;
            remove => _onConnectionOccured -= value; 
        }
        void INetworkCallbackInvoker.ConnectionOccured() => _onConnectionOccured?.Invoke();


        private Action _onConnectionLost;
        event Action INetworkCallbackHandler.OnConnectionLost
        {
            add => _onConnectionLost += value;
            remove => _onConnectionLost -= value;
        }
        void INetworkCallbackInvoker.ConnectionLost() => _onConnectionLost?.Invoke();


        //  ----------------------------------------    Messages
        private Action<IMessage> _onReceivedNetworkMessage;
        event Action<IMessage> INetworkCallbackHandler.OnReceivedNetworkMessage
        {
            add => _onReceivedNetworkMessage += value;
            remove => _onReceivedNetworkMessage -= value;
        }
        void INetworkCallbackInvoker.ReceivedNetworkMessage(IMessage message) => _onReceivedNetworkMessage?.Invoke(message);


        //  ----------------------------------------    Regions
        private Action<int> _onConnectTionToRegionOccured;
        event Action<int> INetworkCallbackHandler.OnConnectionToRegionOccured
        {
            add => _onConnectTionToRegionOccured += value;
            remove => _onConnectTionToRegionOccured -= value;
        }
        void INetworkCallbackInvoker.ConnectionToRegionOccured(int regionId) => _onConnectTionToRegionOccured?.Invoke(regionId);


        private Action<int> _onConnectTionToRegionFailed;
        event Action<int> INetworkCallbackHandler.OnConnectionToRegionFailed
        {
            add => _onConnectTionToRegionFailed += value;
            remove => _onConnectTionToRegionFailed -= value;
        }
        void INetworkCallbackInvoker.ConnectionToRegionFailed(int regionId) => _onConnectTionToRegionFailed?.Invoke(regionId);


        private Action<INetworkRoomBs[]> _onRoomCollectionUpdated;
        event Action<INetworkRoomBs[]> INetworkCallbackHandler.OnRoomCollectionUpdated
        {
            add => _onRoomCollectionUpdated += value;
            remove => _onRoomCollectionUpdated -= value;
        }
        void INetworkCallbackInvoker.RoomCollectionUpdated(INetworkRoomBs[] roomCollection) => _onRoomCollectionUpdated?.Invoke(roomCollection);


        //  ----------------------------------------    Lobby
        private Action<int> _onJoinLobbyOccured;
        event Action<int> INetworkCallbackHandler.OnJoinLobbyOccured
        {
            add => _onJoinLobbyOccured += value;
            remove => _onJoinLobbyOccured -= value;
        }
        void INetworkCallbackInvoker.JoinLobbyOccured(int lobbyId) => _onJoinLobbyOccured?.Invoke(lobbyId);


        private Action<int> _onJoinLobbyFailed;
        event Action<int> INetworkCallbackHandler.OnJoinLobbyFailed
        {
            add => _onJoinLobbyFailed += value;
            remove => _onJoinLobbyFailed -= value;
        }
        void INetworkCallbackInvoker.JoinLobbyFailed(int lobbyId) => _onJoinLobbyFailed?.Invoke(lobbyId);


        //  ----------------------------------------    Room
        private Action<INetworkRoomBs> _onRoomCreationOccured;
        event Action<INetworkRoomBs> INetworkCallbackHandler.OnRoomCreationOccured
        {
            add => _onRoomCreationOccured += value;
            remove => _onRoomCreationOccured -= value;
        }
        void INetworkCallbackInvoker.RoomCreationOccured(INetworkRoomBs roomData) => _onRoomCreationOccured?.Invoke(roomData);


        private Action _onRoomCreationFailed;
        event Action INetworkCallbackHandler.OnRoomCreationFailed
        {
            add => _onRoomCreationFailed += value;
            remove => _onRoomCreationFailed -= value;
        }
        void INetworkCallbackInvoker.RoomCreationFailed() => _onRoomCreationFailed?.Invoke();


        private Action _onLeaveRoomOccured;
        event Action INetworkCallbackHandler.OnLeaveRoomOccured
        {
            add => _onLeaveRoomOccured += value;
            remove => _onLeaveRoomOccured -= value;
        }
        void INetworkCallbackInvoker.LeaveRoomOccured() => _onLeaveRoomOccured?.Invoke();


        private Action<INetworkRoomBs> _onJoinRoomOccured;
        event Action<INetworkRoomBs> INetworkCallbackHandler.OnJoinRoomOccured
        {
            add => _onJoinRoomOccured += value;
            remove => _onJoinRoomOccured -= value;
        }
        void INetworkCallbackInvoker.JoinRoomOccured(INetworkRoomBs roomData) => _onJoinRoomOccured?.Invoke(roomData);


        private Action<string> _onJoinRoomFailed;
        event Action<string> INetworkCallbackHandler.OnJoinRoomFailed
        {
            add => _onJoinRoomFailed += value;
            remove => _onJoinRoomFailed -= value;
        }
        void INetworkCallbackInvoker.JoinRoomFailed(string roomName) => _onJoinRoomFailed?.Invoke(roomName);


        private Action<INetworkPlayerBs> _onAnotherPlayerJoinedTheRoom;
        event Action<INetworkPlayerBs> INetworkCallbackHandler.OnAnotherPlayerJoinedTheRoom
        {
            add => _onAnotherPlayerJoinedTheRoom += value;
            remove => _onAnotherPlayerJoinedTheRoom -= value;
        }
        void INetworkCallbackInvoker.AnotherPlayerJoinedTheRoom(INetworkPlayerBs playerData) => _onAnotherPlayerJoinedTheRoom?.Invoke(playerData);


        private Action<INetworkPlayerBs> _onAnotherPlayerLeftTheRoom;
        event Action<INetworkPlayerBs> INetworkCallbackHandler.OnAnotherPlayerLeftTheRoom
        {
            add => _onAnotherPlayerLeftTheRoom += value;
            remove => _onAnotherPlayerLeftTheRoom -= value;
        }
        void INetworkCallbackInvoker.AnotherPlayerLeftTheRoom(INetworkPlayerBs playerData) => _onAnotherPlayerLeftTheRoom?.Invoke(playerData);


        private Action<INetworkPlayerBs> _onMasterClientSwitched;
        event Action<INetworkPlayerBs> INetworkCallbackHandler.OnMasterClientSwitched
        {
            add => _onMasterClientSwitched += value;
            remove => _onMasterClientSwitched -= value;
        }
        void INetworkCallbackInvoker.MasterClientSwitched(INetworkPlayerBs playerData) => _onMasterClientSwitched?.Invoke(playerData);


        private Action<INetworkRoomBs> _onRoomPropertiesUpdated;
        event Action<INetworkRoomBs> INetworkCallbackHandler.OnRoomPropertiesUpdated
        {
            add => _onRoomPropertiesUpdated += value;
            remove => _onRoomPropertiesUpdated -= value;
        }
        void INetworkCallbackInvoker.RoomPropertiesUpdated(INetworkRoomBs roomData) => _onRoomPropertiesUpdated?.Invoke(roomData);


        //  ----------------------------------------    Ping
        private Action<Dictionary<int, int>> _onPingDataUpdated;
        event Action<Dictionary<int, int>> INetworkCallbackHandler.OnPingDataUpdated
        {
            add => _onPingDataUpdated += value;
            remove => _onPingDataUpdated -= value;
        }
        void INetworkCallbackInvoker.PingDataUpdated(Dictionary<int, int> pingDataCollection) => _onPingDataUpdated?.Invoke(pingDataCollection);


        //  ----------------------------------------    Player
        private Action<INetworkPlayerBs> _onPlayerDataUpdated;
        event Action<INetworkPlayerBs> INetworkCallbackHandler.OnPlayerDataUpdated
        {
            add => _onPlayerDataUpdated += value;
            remove => _onPlayerDataUpdated -= value;
        }
        void INetworkCallbackInvoker.PlayerDataUpdated(INetworkPlayerBs playerData) => _onPlayerDataUpdated?.Invoke(playerData);


        private Func<string> _onGetLocalPlayerLogInName;
        event Func<string> INetworkCallbackHandler.OnGetLocalPlayerLogInName
        {
            add => _onGetLocalPlayerLogInName += value;
            remove => _onGetLocalPlayerLogInName -= value;
        }
        string INetworkCallbackInvoker.GetLocalPlayerLogInName() => _onGetLocalPlayerLogInName?.Invoke();


        //  ----------------------------------------    Previous connection data
        private Func<int?> _onGetPreviousConnectedLobbyId;
        event Func<int?> INetworkCallbackHandler.OnGetPreviousConnectedLobbyId
        {
            add => _onGetPreviousConnectedLobbyId += value;
            remove => _onGetPreviousConnectedLobbyId -= value;
        }
        int? INetworkCallbackInvoker.GetPreviousConnectedLobbyId() => _onGetPreviousConnectedLobbyId?.Invoke();


        private Func<int?> _onGetPreviousConnectedRegionId;
        event Func<int?> INetworkCallbackHandler.OnGetPreviousConnectedRegionId
        {
            add => _onGetPreviousConnectedRegionId += value;
            remove => _onGetPreviousConnectedRegionId -= value;
        }
        int? INetworkCallbackInvoker.GetPreviousConnectedRegionId() => _onGetPreviousConnectedRegionId?.Invoke();

        #endregion


        //  ----------------------------------------    Network api event handler
        #region Network api event handler

        private Action<int[],float?> _onInitializeNetwork;
        event Action<int[], float?> INetworkEventsHandler.OnInitializeNetwork
        {
            add => _onInitializeNetwork += value;
            remove => _onInitializeNetwork -= value;
        }
        void INetworkEventsInvoker.InitializeNetwork(int[] lobbyId, float? waitingConnectingSeconds) => _onInitializeNetwork?.Invoke(lobbyId, waitingConnectingSeconds);


        private Func<bool> _onIsConnected;
        event Func<bool> INetworkEventsHandler.OnIsConnected
        {
            add => _onIsConnected += value;
            remove => _onIsConnected -= value;
        }
        bool INetworkEventsInvoker.IsConnected()
        {
            var result = _onIsConnected?.Invoke();
            return result == null ? false : result.Value;
        }


        private Action<float> _onSetWaitingConnectionSeconds;
        event Action<float> INetworkEventsHandler.OnSetWaitingConnectionSeconds
        {
            add => _onSetWaitingConnectionSeconds += value;
            remove => _onSetWaitingConnectionSeconds -= value;
        }
        void INetworkEventsInvoker.SetWaitingConnectionSeconds(float seconds) => _onSetWaitingConnectionSeconds?.Invoke(seconds);


        //  ----------------------------------------    Messages
        private Action<IMessage> _onSendNetworkMessage;
        event Action<IMessage> INetworkEventsHandler.OnSendNetworkMessage
        {
            add => _onSendNetworkMessage += value;
            remove => _onSendNetworkMessage -= value;
        }
        void INetworkEventsInvoker.SendNetworkMessage(IMessage message) => _onSendNetworkMessage?.Invoke(message);


        //  ----------------------------------------    Regions
        private Action<int> _onConnectToRegion;
        event Action<int> INetworkEventsHandler.OnConnectToRegion
        {
            add => _onConnectToRegion += value;
            remove => _onConnectToRegion -= value;
        }
        void INetworkEventsInvoker.ConnectToRegion(int regionId) => _onConnectToRegion?.Invoke(regionId);


        private Func<int> _onGetCurrentRegionId;
        event Func<int> INetworkEventsHandler.OnGetCurrentRegionId
        {
            add => _onGetCurrentRegionId += value;
            remove => _onGetCurrentRegionId -= value;
        }
        int? INetworkEventsInvoker.GetCurrentRegionId() => _onGetCurrentRegionId?.Invoke();


        private Func<int,string> _onGetRegionNameById;
        event Func<int,string> INetworkEventsHandler.OnGetRegionNameById
        {
            add => _onGetRegionNameById += value;
            remove => _onGetRegionNameById -= value;
        }
        string INetworkEventsInvoker.GetRegionNameById(int regionId) => _onGetRegionNameById?.Invoke(regionId);


        private Func<string, int> _onGetRegionIdByName;
        event Func<string, int> INetworkEventsHandler.OnGetRegionIdByName
        {
            add => _onGetRegionIdByName += value;
            remove => _onGetRegionIdByName -= value;
        }
        int? INetworkEventsInvoker.GetRegionIdByName(string regionName) => _onGetRegionIdByName?.Invoke(regionName);

        private Func<int, string> _onGetLocalizedRegionNameById;
        event Func<int, string> INetworkEventsHandler.OnGetLocalizedRegionNameById
        {
            add => _onGetLocalizedRegionNameById += value;
            remove => _onGetLocalizedRegionNameById -= value;
        }
        public string GetLocalizedRegionNameById(int regionId) => _onGetLocalizedRegionNameById?.Invoke(regionId);

        private Func<string, int?> _onGetRegionIdByLocalizedName;
        event Func<string, int? > INetworkEventsHandler.OnGetRegionIdByLocalizedName
        {
            add => _onGetRegionIdByLocalizedName += value;
            remove => _onGetRegionIdByLocalizedName -= value;
        }
        public int? GetRegionIdByLocalizedName(string regionName) => _onGetRegionIdByLocalizedName?.Invoke(regionName); 

        //  ----------------------------------------    Lobby
        private Func<bool> _onIsInLobby;
        event Func<bool> INetworkEventsHandler.OnIsInLobby
        {
            add => _onIsInLobby += value;
            remove => _onIsInLobby -= value;
        }
        bool INetworkEventsInvoker.IsInLobby()
        {
            var result = _onIsInLobby?.Invoke();
            return result == null ? false : result.Value;
        }


        private Action<int> _onJoinLobby;
        event Action<int> INetworkEventsHandler.OnJoinLobby
        {
            add => _onJoinLobby += value;
            remove => _onJoinLobby -= value;
        }
        void INetworkEventsInvoker.JoinLobby(int lobbyId) => _onJoinLobby?.Invoke(lobbyId);


        private Func<int> _onGetCurrentLobbyId;
        event Func<int> INetworkEventsHandler.OnGetCurrentLobbyId
        {
            add => _onGetCurrentLobbyId += value;
            remove => _onGetCurrentLobbyId -= value;
        }
        int? INetworkEventsInvoker.GetCurrentLobbyId() => _onGetCurrentLobbyId?.Invoke();


        private Func<INetworkRoomBs[]> _onGetRoomCollection;
        event Func<INetworkRoomBs[]> INetworkEventsHandler.OnGetRoomCollection
        {
            add => _onGetRoomCollection += value;
            remove => _onGetRoomCollection -= value;
        }
        INetworkRoomBs[] INetworkEventsInvoker.GetRoomCollection() => _onGetRoomCollection?.Invoke();


        //  ----------------------------------------    Room
        private Action<INetworkRoomBs> _onCreateNewRoom;
        event Action<INetworkRoomBs> INetworkEventsHandler.OnCreateNewRoom
        {
            add => _onCreateNewRoom += value;
            remove => _onCreateNewRoom -= value;
        }
        void INetworkEventsInvoker.CreateNewRoom(INetworkRoomBs newRoomData) => _onCreateNewRoom?.Invoke(newRoomData);


        private Action _onLeaveRoom;
        event Action INetworkEventsHandler.OnLeaveRoom
        {
            add => _onLeaveRoom += value;
            remove => _onLeaveRoom -= value;
        }
        void INetworkEventsInvoker.LeaveRoom() => _onLeaveRoom?.Invoke();


        private Action<string> _onJoinRoom;
        event Action<string> INetworkEventsHandler.OnJoinRoom
        {
            add => _onJoinRoom += value;
            remove => _onJoinRoom -= value;
        }
        void INetworkEventsInvoker.JoinRoom(string roomName) => _onJoinRoom?.Invoke(roomName);


        private Func<bool> _onIsInsideTheRoom;
        event Func<bool> INetworkEventsHandler.OnIsInsideTheRoom
        {
            add => _onIsInsideTheRoom += value;
            remove => _onIsInsideTheRoom -= value;
        }
        bool INetworkEventsInvoker.IsInsideTheRoom()
        {
            var result = _onIsInsideTheRoom?.Invoke();
            return result == null ? false : result.Value;
        }


        private Func<INetworkPlayerBs[]> _onGetPlayersInsideCurrentRoom;
        event Func<INetworkPlayerBs[]> INetworkEventsHandler.OnGetPlayersInsideCurrentRoom
        {
            add => _onGetPlayersInsideCurrentRoom += value;
            remove => _onGetPlayersInsideCurrentRoom -= value;
        }
        INetworkPlayerBs[] INetworkEventsInvoker.GetPlayersInsideCurrentRoom() => _onGetPlayersInsideCurrentRoom?.Invoke();


        private Action<int> _onKickPlayerFromRoom;
        event Action<int> INetworkEventsHandler.OnKickPlayerFromRoom
        {
            add => _onKickPlayerFromRoom += value;
            remove => _onKickPlayerFromRoom -= value;
        }
        void INetworkEventsInvoker.KickPlayerFromRoom(int playerGameSessionNetworkId) => _onKickPlayerFromRoom?.Invoke(playerGameSessionNetworkId);


        private Action<INetworkRoomBs> _onCopyDataInToTheCurrentRoom;
        event Action<INetworkRoomBs> INetworkEventsHandler.OnCopyPropertiesInToTheCurrentRoom
        {
            add => _onCopyDataInToTheCurrentRoom += value;
            remove => _onCopyDataInToTheCurrentRoom -= value;
        }
        void INetworkEventsInvoker.CopyDataInToTheCurrentRoom(INetworkRoomBs roomData) => _onCopyDataInToTheCurrentRoom?.Invoke(roomData);


        private Func<INetworkRoomBs> _onGetCurrentRoomData;
        event Func<INetworkRoomBs> INetworkEventsHandler.OnGetCurrentRoomData
        {
            add => _onGetCurrentRoomData += value;
            remove => _onGetCurrentRoomData -= value;
        }
        INetworkRoomBs INetworkEventsInvoker.GetCurrentRoomData() => _onGetCurrentRoomData?.Invoke();


        //  ----------------------------------------    Ping
        private Func<int> _onGetCurrentRegionPing;
        event Func<int> INetworkEventsHandler.OnGetCurrentRegionPing
        {
            add => _onGetCurrentRegionPing += value;
            remove => _onGetCurrentRegionPing -= value;
        }
        int? INetworkEventsInvoker.GetCurrentRegionPing() => _onGetCurrentRegionPing?.Invoke();


        private Func<Dictionary<int, int>> _onGetRegionsPingCollection;
        event Func<Dictionary<int, int>> INetworkEventsHandler.OnGetRegionsPingCollection
        {
            add => _onGetRegionsPingCollection += value;
            remove => _onGetRegionsPingCollection -= value;
        }
        Dictionary<int, int> INetworkEventsInvoker.GetRegionsPingCollection() => _onGetRegionsPingCollection?.Invoke();


        //  ----------------------------------------    Player
        private Func<bool> _onOnIsMasterClient;
        event Func<bool> INetworkEventsHandler.OnIsMasterClient
        {
            add => _onOnIsMasterClient += value;
            remove => _onOnIsMasterClient -= value;
        }
        bool? INetworkEventsInvoker.IsMasterClient() => _onOnIsMasterClient?.Invoke();


        private Action<INetworkPlayerBs> _onCopyInToTheLocalPlayerData;
        event Action<INetworkPlayerBs> INetworkEventsHandler.OnCopyInToTheLocalPlayerData
        {
            add => _onCopyInToTheLocalPlayerData += value;
            remove => _onCopyInToTheLocalPlayerData -= value;
        }
        void INetworkEventsInvoker.CopyInToTheLocalPlayerData(INetworkPlayerBs playerData) => _onCopyInToTheLocalPlayerData?.Invoke(playerData);


        private Func<INetworkPlayerBs> _onGetLocalPlayerData;
        event Func<INetworkPlayerBs> INetworkEventsHandler.OnGetLocalPlayerData
        {
            add => _onGetLocalPlayerData += value;
            remove => _onGetLocalPlayerData -= value;
        }
        INetworkPlayerBs INetworkEventsInvoker.GetLocalPlayerData() => _onGetLocalPlayerData?.Invoke();


        private Func<INetworkPlayerBs> _onGetMasterPlayerData;
        event Func<INetworkPlayerBs> INetworkEventsHandler.OnGetMasterPlayerData
        {
            add => _onGetMasterPlayerData += value;
            remove => _onGetMasterPlayerData -= value;
        }
        INetworkPlayerBs INetworkEventsInvoker.GetMasterPlayerData() => _onGetMasterPlayerData?.Invoke();

        //  ----------------------------------------    Previous connection data
        #endregion
    }
}
