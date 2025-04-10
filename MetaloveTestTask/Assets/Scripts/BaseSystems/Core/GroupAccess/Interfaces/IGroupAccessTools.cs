using System.Collections.Generic;

namespace Scripts.BaseSystems.GameObjListTools
{
    public interface IGroupAccessTools 
    {
        public bool GetGroupActiveState(GroupAccessId groupId);
        public void Register(IGroupAccess iGroupAccess);
        public void Unregister(IGroupAccess iGroupAccess);
        public void ChangeGroupActiveState(GroupAccessId groupId, bool newState, bool turnOtherGroupsOff = false);
        public Dictionary<GroupAccessId, bool> GetAllGroupActiveState();
        public void MemorizeAllRegisteredObjectsStateFor(int instanceId, object obj);
        public void ForgetAllRegisteredObjStateFor(int instanceId);
        public void ForgetAllRegisteredObjStateFor(UnityEngine.Object obj);
        public void RestoreAllRegisteredObjectStateByMemory(int instanceId , bool removeFromMemory = false);
    }
}
