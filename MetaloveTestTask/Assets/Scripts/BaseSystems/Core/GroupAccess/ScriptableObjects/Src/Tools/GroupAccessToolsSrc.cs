using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Scripts.BaseSystems.GameObjListTools
{
    [CreateAssetMenu(menuName = "Scriptable Obj/Base systems/Core/Group access/Group access tools")]
    public class GroupAccessToolsSrc : ScriptableObject, IGroupAccessTools
    {
        [SerializeField, Header("Array visualisation in inspector only for visualisation")]
        private GroupDescription[] _groupDescription;

        private Dictionary<GroupAccessId, GroupDescription> _groupDictionary = new Dictionary<GroupAccessId, GroupDescription>();

        //  Ask key is used game object InstanceID, dictionary provides group where object with instanceId is registered
        private Dictionary<int, GroupDescription> _instanceIdGroupDictionary = new Dictionary<int, GroupDescription>();

        //  Key is game object instance id
        private Dictionary<int, MemoryUnit> MemorizedRegisteredGroupState { get; set; } = new Dictionary<int, MemoryUnit>(); 

        #region Internal classes
        [Serializable]
        private class GroupDescription
        {
            [SerializeField]
            public string _name;
            [SerializeField]
            public GroupAccessId _groupId;
            [SerializeField]
            public List<GameObject> GameObjectList { get; set; } = new List<GameObject>();

            //  As key is used game object InstanceID
            public Dictionary<int, GameObject> GameObjectDictionary { get; set; } = new Dictionary<int, GameObject>();

            private bool _activeState;
            public bool ActiveState
            {
                get => _activeState;

                set
                {
                    Debug.Log("\t\t\t group: " + _groupId + "\t elements amount: " + GameObjectList.Count);
                    for (int i = 0; i < GameObjectList.Count; i++)
                    {
                        Debug.Log("\t\t\t\t [ " + i + " ] " + GameObjectList[i].name + "\t " + value);
                        GameObjectList[i].SetActive(value);
                    }

                    _activeState = value;
                }
            }
            public GroupDescription(GroupAccessId groupId)
            {
                _groupId = groupId;
            }

            public void Add(GameObject obj)
            {
                var instanceId = obj.GetInstanceID();
                if (!GameObjectDictionary.ContainsKey(instanceId))
                {
                    GameObjectDictionary.Add(instanceId, obj);
                    GameObjectList.Add(obj);
                }
            }

            public void Remove(GameObject obj)
            {
                var instanceId = obj.GetInstanceID();
                if (GameObjectDictionary.ContainsKey(instanceId))
                {
                    GameObjectDictionary.Remove(instanceId);
                    GameObjectList.Remove(obj);
                }
            }

            public void Remove(int instanceId)
            {
                if (GameObjectDictionary.ContainsKey(instanceId))
                {
                    var gameObject = GameObjectDictionary[instanceId];
                    GameObjectDictionary.Remove(instanceId);
                    GameObjectList.Remove(gameObject);
                }
            }

            public int[] GetNullInstanceIds()
            {
                var instanceIdThatAreNull = new List<int>();

                foreach (var item in GameObjectDictionary)
                    if (item.Value == null)
                        instanceIdThatAreNull.Add(item.Key);

                return instanceIdThatAreNull.ToArray();
            }
        }

        private struct MemoryUnit
        {
            public int _instanceId;
            public Dictionary<GroupAccessId, bool> _registeredGroupState;
            public object _obj; 

            public MemoryUnit(int instanceId, object obj, Dictionary<GroupAccessId, bool> registeredGroupState)
            {
                _instanceId = instanceId;
                _obj = obj;
                _registeredGroupState = new Dictionary<GroupAccessId, bool>(registeredGroupState); 
            }
        }

        #endregion

        private IGroupAccessTools _iGroupAccessTools;
        private IGroupAccessTools IGroupAccessTools
        {
            get
            {
                if (_iGroupAccessTools == null)
                    _iGroupAccessTools = this;
                return _iGroupAccessTools;
            }
        }

        void IGroupAccessTools.ChangeGroupActiveState(GroupAccessId groupId, bool newState, bool turnOtherGroupsOff)
        {
            if (!_groupDictionary.ContainsKey(groupId))
                _groupDictionary.Add(groupId, new GroupDescription(groupId));

            if(turnOtherGroupsOff)
            {
                foreach (var item in _groupDictionary)
                {
                    if (item.Key != groupId)
                        item.Value.ActiveState = false;
                }
            }

            Debug.Log("\t\t ChangeGroupActiveState group: " + groupId+"\t state: "+newState); 
            _groupDictionary[groupId].ActiveState = newState;
        }

        void IGroupAccessTools.Register(IGroupAccess iGroupAccess)
        {
            Debug.Log("\t\t Registering access group id: "+ iGroupAccess.GroupAccessId);

            VerifyAndClear();

            var groupId = iGroupAccess.GroupAccessId;
            var instanceGameObjectId = iGroupAccess.GameObject.GetInstanceID();

            if (!_groupDictionary.ContainsKey(groupId))
                _groupDictionary.Add(groupId, new GroupDescription(groupId));

            _groupDictionary[groupId].Add(iGroupAccess.GameObject);

            if (!_instanceIdGroupDictionary.ContainsKey(instanceGameObjectId))
                _instanceIdGroupDictionary.Add(instanceGameObjectId, _groupDictionary[groupId]);
        }

        void IGroupAccessTools.Unregister(IGroupAccess iGroupAccess)
        {
            var groupId = iGroupAccess.GroupAccessId;
            var instanceGameObjectId = iGroupAccess.GameObject.GetInstanceID();

            if (_groupDictionary.ContainsKey(groupId))
                _groupDictionary[groupId].Remove(instanceGameObjectId);

            if (_instanceIdGroupDictionary.ContainsKey(instanceGameObjectId))
                _instanceIdGroupDictionary.Remove(instanceGameObjectId);
        }

        bool IGroupAccessTools.GetGroupActiveState(GroupAccessId groupId)
        {
            if (_groupDictionary.ContainsKey(groupId))
                return _groupDictionary[groupId].ActiveState;
            
            return false;
        }

        private async void VerifyAndClear()
        {
            var instanceIdListThatAreDestroyed = new List<int>();

            foreach (var group in _groupDictionary)
            {
                foreach (var item in group.Value.GameObjectDictionary)
                    if (item.Value == null)
                        instanceIdListThatAreDestroyed.Add(item.Key);
            }

            for (int i = 0; i < instanceIdListThatAreDestroyed.Count; i++)
            {
                var instanceIdToRemove = instanceIdListThatAreDestroyed[i];
                _instanceIdGroupDictionary[instanceIdToRemove].Remove(instanceIdToRemove);
                _instanceIdGroupDictionary.Remove(instanceIdToRemove);
            }

            await Task.Yield();
        }

        Dictionary<GroupAccessId, bool> IGroupAccessTools.GetAllGroupActiveState()
        {
            var groupActiveStateDictionary = new Dictionary<GroupAccessId, bool>();

            foreach (var item in _groupDictionary)
                groupActiveStateDictionary.Add(item.Key, item.Value.ActiveState);

            return groupActiveStateDictionary;
        }

        /// <summary>
        ///     instanceId is a key to memory disctionary
        ///     obj is and object that will be verified, so memory can be cleared is object was destroyed
        /// </summary>
        /// <param name="instanceId"></param>
        /// <param name="obj"></param>
        void IGroupAccessTools.MemorizeAllRegisteredObjectsStateFor(int instanceId, object obj)
        {
            ClearFromNullsMemory(); 

            if (MemorizedRegisteredGroupState.ContainsKey(instanceId))
                MemorizedRegisteredGroupState.Remove(instanceId);

            MemorizedRegisteredGroupState.Add(instanceId, new MemoryUnit(
                                                                        instanceId,
                                                                        obj,
                                                                        IGroupAccessTools.GetAllGroupActiveState()
                                                                        )
                                                ); 
        }

        void IGroupAccessTools.ForgetAllRegisteredObjStateFor(int instanceId)
        {
            if (MemorizedRegisteredGroupState.ContainsKey(instanceId))
                MemorizedRegisteredGroupState.Remove(instanceId);
        }

        void IGroupAccessTools.ForgetAllRegisteredObjStateFor(UnityEngine.Object obj)
        {
            var instanceId = obj.GetInstanceID(); 
            if (MemorizedRegisteredGroupState.ContainsKey(instanceId))
                MemorizedRegisteredGroupState.Remove(instanceId);
        }

        private void ClearFromNullsMemory()
        {
            List<int> _instanceIdThatNeedToBeRemoved = new List<int>();

            foreach (var item in MemorizedRegisteredGroupState)
            {
                if (item.Value._obj == null)
                    _instanceIdThatNeedToBeRemoved.Add(item.Key); 
            }

            for (int i = 0; i < _instanceIdThatNeedToBeRemoved.Count; i++)
                MemorizedRegisteredGroupState.Remove(_instanceIdThatNeedToBeRemoved[i]); 
        }

        void IGroupAccessTools.RestoreAllRegisteredObjectStateByMemory(int instanceId, bool removeFromMemory )
        {
            if (!MemorizedRegisteredGroupState.ContainsKey(instanceId)) return; 

            var memoryGroup = MemorizedRegisteredGroupState[instanceId]._registeredGroupState;

            foreach (var item in memoryGroup)
                IGroupAccessTools.ChangeGroupActiveState(item.Key, item.Value);

            if (removeFromMemory)
                MemorizedRegisteredGroupState.Remove(instanceId); 
        }
    }
}

