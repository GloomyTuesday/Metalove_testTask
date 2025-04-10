using System;
using System.Collections.Generic;

namespace Scripts.BaseSystems.NetworkBase
{
    [Serializable]
    public class NetworkRoomBs : INetworkRoomBs
    {
        private string _roomId;
        private string _roomName;
        private int _playersAmount;
        private int _maxPlayersAmount;
        private int _lobbyId;

        private Dictionary<object, object> _customProperty = new Dictionary<object, object>();

        public string RoomId => _roomId;
        public string RoomName => _roomName;
        public int PlayerAmount => _playersAmount;
        public int MaxPlayerAmount => _maxPlayersAmount;
        public int LobbyId => _lobbyId;

        public Dictionary<object, object> CustomProperties => _customProperty;

        public NetworkRoomBs(
        string roomId,
        string roomName,
        int playersAmount,
        int maxPlayersAmount,
        int lobbyId, 
        Dictionary<object, object> customPropertie
        )
        {
            _roomId = roomId;
            _roomName = roomName; 
            _playersAmount = playersAmount;
            _maxPlayersAmount = maxPlayersAmount;
            _lobbyId = lobbyId;
            
            foreach (var item in customPropertie)
                _customProperty.Add(item.Key, item.Value);
        }

        public NetworkRoomBs(INetworkRoomBs iRoomDataToCopy )
        {
            CopyDataFrom(iRoomDataToCopy);
        }

        public void CopyDataFrom(INetworkRoomBs roomDataToCopyFrom)
        {
            _roomId = roomDataToCopyFrom.RoomId;
            _roomName = roomDataToCopyFrom.RoomName;
            _playersAmount = roomDataToCopyFrom.PlayerAmount;
            _lobbyId = roomDataToCopyFrom.LobbyId;
            _maxPlayersAmount = roomDataToCopyFrom.MaxPlayerAmount;

            _customProperty.Clear();
            foreach (var item in roomDataToCopyFrom.CustomProperties)
                _customProperty.Add(item.Key, item.Value);

        }
        
        public T GetProperty<T>(object key)
        {
            if (!CustomProperties.ContainsKey(key)) return default;
            if (!(CustomProperties[key] is not T)) return default;
            return (T)CustomProperties[key];
        }
    }
}
