using UnityEngine;

namespace Scripts.BaseSystems.NetworkBase
{
    [CreateAssetMenu(menuName = "Scriptable Obj/Base systems/Core/Network base/New room network bank")]
    public class NetworkNewRoomBankSrc : ScriptableObject
    {
        [SerializeField]
        private string _newRoomName;
        [SerializeField]
        private int _newRoomLobbyId;
        [SerializeField]
        private int _newRoomMaxPlayersAmount; 

        public string NewRoomName { get => _newRoomName; set => _newRoomName = value; }
        public int NewRoomLobbyId { get => _newRoomLobbyId; set => _newRoomLobbyId = value; }
        public int NewRoomMaxPlayersAmount { get => _newRoomMaxPlayersAmount; set => _newRoomMaxPlayersAmount = value; }
    }
}
