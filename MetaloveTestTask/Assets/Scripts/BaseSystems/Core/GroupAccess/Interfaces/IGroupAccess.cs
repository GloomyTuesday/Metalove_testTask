using UnityEngine;

namespace Scripts.BaseSystems.GameObjListTools
{
    public interface IGroupAccess 
    {
        public GameObject GameObject { get; }
        public GroupAccessId GroupAccessId { get; }
    }
}

