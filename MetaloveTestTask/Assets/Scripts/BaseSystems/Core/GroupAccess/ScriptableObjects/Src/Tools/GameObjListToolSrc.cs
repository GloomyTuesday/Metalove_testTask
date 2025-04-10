using UnityEngine;
using System;
using System.Collections.Generic;

namespace Scripts.BaseSystems.GameObjListTools
{
    [CreateAssetMenu(fileName = "GameObjListTool", menuName = "Scriptable Obj/Base systems/Core/Game object list tools/Game bject list tool")]
    public class GameObjListToolSrc : ScriptableObject, IGameObjListTools
    {
        [SerializeField]
        private bool _update;
        [SerializeField]
        private DebugUnit[] _registeredObjArray;

        [NonSerialized]
        private List<RegisteredUnit> _registeredUnits = new List<RegisteredUnit>();

        [NonSerialized]
        private Dictionary<string, RegisteredUnit> RegisteredUnitsDictionary = new Dictionary<string, RegisteredUnit>();
        [NonSerialized]
        private Dictionary<int, string> ObjectInstanceIdToKeyDictionary = new Dictionary<int, string>();

        #region Local units
        [Serializable]
        public class RegisteredUnit
        {
            [Space(5)]
            public string _key = "Key";

            //  As key are used GameObject instance IDs
            private Dictionary<int, GameObject> _activatorGameObjectDictionary = new Dictionary<int, GameObject>();
            private Dictionary<int, GameObject> _activableGameObjectDictionary = new Dictionary<int, GameObject>();

            private List<GameObject> _activatorList = new List<GameObject>();
            public List<GameObject> ActivatorList => _activatorList;

            private List<GameObject> _activalableList = new List<GameObject>();
            public List<GameObject> ActivableList => _activalableList;

            private event Action OnActivate;
            private event Action OnDeactivate;

            private bool _activeState;
            public bool ActivState
            {
                get => _activeState;
                set
                {
                    _activeState = value;

                    if (_activeState)
                    {
                        OnActivate?.Invoke();
                        return;
                    }

                    OnDeactivate?.Invoke();
                }
            }

            public void TryToAddActivator(
                GameObject activatorObj,
                Action activationCallback,
                Action deactivationCallback
                )
            {
                var instanceId = activatorObj.GetInstanceID();

                if (_activatorGameObjectDictionary.ContainsKey(instanceId)) return;

                _activatorGameObjectDictionary.Add(instanceId, activatorObj);
                _activatorList.Add(activatorObj);
                OnActivate += activationCallback;
                OnDeactivate += deactivationCallback;

                if (ActivState)
                    activationCallback?.Invoke();
            }

            public void TryToAddActivable(
                GameObject activableObj,
                Action activationCallback,
                Action deactivationCallback
                )
            {
                var instanceId = activableObj.GetInstanceID();

                if (_activableGameObjectDictionary.ContainsKey(instanceId)) return;

                _activableGameObjectDictionary.Add(instanceId, activableObj);
                _activalableList.Add(activableObj);
                OnActivate += activationCallback;
                OnDeactivate += deactivationCallback;

                if (ActivState)
                    activationCallback?.Invoke();
            }

            public void TryToRemoveActivator(
                GameObject activatorObj,
                Action activationCallback,
                Action deactivationCallback
                )
            {
                OnActivate -= activationCallback;
                OnDeactivate -= deactivationCallback;

                if (activatorObj == null) return; 

                var instanceId = activatorObj.GetInstanceID();

                if (!_activatorGameObjectDictionary.ContainsKey(instanceId)) return;
                
                _activatorGameObjectDictionary.Remove(instanceId);
                _activatorList.Remove(activatorObj);
            }

            public void TryToRemoveActivable(
                GameObject activableObj,
                Action activationCallback,
                Action deactivationCallback
                )
            {
                OnActivate -= activationCallback;
                OnDeactivate -= deactivationCallback;

                if (activableObj == null) return;
                var instanceId = activableObj.GetInstanceID();
                if (!_activableGameObjectDictionary.ContainsKey(instanceId)) return;

                _activableGameObjectDictionary.Remove(instanceId);
                _activalableList.Remove(activableObj);
            }
        }


        /// <summary>
        ///     Debug unit
        /// </summary>
        /// 
        [Serializable]
        private class DebugUnit
        {
            public string _key;

            public GameObject[] _activatorCollection;
            public GameObject[] _activableCollection;
        }

        #endregion


        private void OnValidate()
        {
            if (_update)
            {
                _update = false;
                UpdateDebugData();
            }
        }

        void IGameObjListTools.TryToRegisterActivator(
            string key,
            Action activationCallback,
            Action deactivationCallback,
            GameObject obj
            )
        {
            var unit = GetUnit(key);
            unit.TryToRemoveActivator(obj, activationCallback, deactivationCallback);

            var instanceId = obj.GetInstanceID();

            if (!ObjectInstanceIdToKeyDictionary.ContainsKey(instanceId))
                ObjectInstanceIdToKeyDictionary.Add(instanceId, key);
        }

        void IGameObjListTools.TryToUnregisterActivator(
            string key,
            Action activationCallback,
            Action deactivationCallback,
            GameObject obj
            )
        {
            var unit = GetUnit(key);
            unit.TryToRemoveActivator(obj, activationCallback, deactivationCallback);

            var instanceId = obj.GetInstanceID();
            ObjectInstanceIdToKeyDictionary.Remove(instanceId);

            if (unit.ActivatorList.Count < 1 && unit.ActivableList.Count < 1)
                RemoveUnit(key);
        }

        void IGameObjListTools.TryToRegisterActivalable(
            string key,
            Action activationCallback,
            Action deactivationCallback,
            GameObject obj
            )
        {
            var unit = GetUnit(key);
            unit.TryToAddActivable(obj, activationCallback, deactivationCallback);
        }

        void IGameObjListTools.TryToUnregisterActivalable(
            string key,
            Action activationCallback,
            Action deactivationCallback,
            GameObject obj
            )
        {
            var unit = GetUnit(key);
            unit.TryToRemoveActivable(obj, activationCallback, deactivationCallback);

            if (unit.ActivatorList.Count < 1 && unit.ActivableList.Count < 1)
                RemoveUnit(key);
        }

        public void Activate(string key)
        {

            Debug.Log("\t Activate request for ky: "+key);

            if (!RegisteredUnitsDictionary.ContainsKey(key))
            {
                Debug.LogError("Attempt to activate missing key: " + key);
                return;
            }

            ActivateCertainGroup(key); 
        }

        public void Deactiate(string key)
        {
            if (!RegisteredUnitsDictionary.ContainsKey(key))
            {
                Debug.LogError("Attempt to activate missing key: " + key);
                return;
            }

            var unit = GetUnit(key);
            unit.ActivState = false ;
        }

        public void DiactivateAll()
        {
            foreach (var item in RegisteredUnitsDictionary)
                item.Value.ActivState = false;
        }

        private RegisteredUnit GetUnit(string key)
        {
            if (!RegisteredUnitsDictionary.ContainsKey(key))
            {
                RegisteredUnitsDictionary.Add(
                                                key,
                                                new RegisteredUnit()
                                                { _key = key }
                                            );
                AddElementToMainUnitList(RegisteredUnitsDictionary[key]);
            }

            return RegisteredUnitsDictionary[key];
        }

        private void AddElementToMainUnitList(RegisteredUnit registeredUnit)
        {
            _registeredUnits.Add(registeredUnit);

#if UNITY_EDITOR
            UpdateDebugData();
#endif
        }

        private void RemoveElementFromMainUnitList(RegisteredUnit registeredUnit)
        {
            _registeredUnits.Remove(registeredUnit);
#if UNITY_EDITOR
            UpdateDebugData();
#endif
        }

        private void UpdateDebugData()
        {
            _registeredObjArray = new DebugUnit[RegisteredUnitsDictionary.Count];
            int count = 0;

            foreach (var item in RegisteredUnitsDictionary)
            {
                _registeredObjArray[count] = new DebugUnit();
                _registeredObjArray[count]._key = item.Key;
                _registeredObjArray[count]._activatorCollection = item.Value.ActivatorList.ToArray();
                _registeredObjArray[count]._activableCollection = item.Value.ActivableList.ToArray();

                count++;
            }
        }

        private void RemoveElementFromMainUnitList(string key)
        {
            for (int i = _registeredUnits.Count - 1; i >= 0; i--)
            {
                if (_registeredUnits[i]._key == key)
                {
                    _registeredUnits.RemoveAt(i);
                    return;
                }
            }
        }

        private void RemoveUnit(string key)
        {
            RegisteredUnit unit = null;

            if (RegisteredUnitsDictionary.ContainsKey(key))
                unit = RegisteredUnitsDictionary[key];

            if (unit != null)
            {
                RemoveElementFromMainUnitList(unit);
                return;
            }

            RemoveElementFromMainUnitList(key);
        }

        public void ActivateCertainGroup(string key)
        {
            if (!RegisteredUnitsDictionary.ContainsKey(key))
            {
                Debug.LogError("Missing group wth key: "+key);
                return; 
            }

            foreach (var item in RegisteredUnitsDictionary)
                item.Value.ActivState = false;

            RegisteredUnitsDictionary[key].ActivState= true;
        }
    }
}
