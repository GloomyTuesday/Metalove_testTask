using System.Collections.Generic;

namespace Scripts.BaseSystems.NetworkBase
{
    public interface INetworkRoomBs
    {
        public string RoomId { get; }
        public string RoomName { get; }
        public int PlayerAmount { get; }
        public int MaxPlayerAmount { get; }
        public int LobbyId { get; }


        public Dictionary<object, object> CustomProperties { get; }

        public T GetProperty<T>(object key);

    }
}
