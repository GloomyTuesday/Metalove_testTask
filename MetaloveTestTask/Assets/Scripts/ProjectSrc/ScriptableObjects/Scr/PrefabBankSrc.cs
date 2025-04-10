using Scripts.BaseSystems;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Scripts.ProjectSrc
{
    [CreateAssetMenu(fileName = "PrefabBank", menuName = "Scriptable Obj/Project src/Prefab bank")]
    public class PrefabBankSrc : ScriptableObject, IBankTypeId<string, GameObject>
    {
        [SerializeField]
        private List<ItemDescription> _itemDescription;

        [NonSerialized]
        private bool _ready;

        private List<GameObject> _scenarioScriptList;
        public List<GameObject> ScenarioScriptList
        {
            get
            {
                if (!_ready)
                    Init();

                return _scenarioScriptList;
            }
        }

        private Dictionary<string, GameObject> _scenarioScriptDictionary;
        private Dictionary<string, GameObject> ScenarioScriptDictionary
        {
            get
            {
                if (!_ready)
                    Init();

                return _scenarioScriptDictionary;
            }
        }

        public event Action<string> OnItemRemoved;
        public event Action<string> OnItemAdded;

        private void OnValidate()
        {
            if (_itemDescription == null) return;

            for (int i = 0; i < _itemDescription.Count; i++)
                _itemDescription[i]._id = _itemDescription[i]._prefab.name; 
        }

        private void Init()
        {
            _scenarioScriptDictionary = new Dictionary<string, GameObject>();
            _scenarioScriptList = new List<GameObject>();

            for (int i = 0; i < _itemDescription.Count; i++)
            {
                _scenarioScriptList.Add(_itemDescription[i]._prefab);

                if (_scenarioScriptDictionary.ContainsKey(_itemDescription[i]._id))
                {
                    Debug.LogWarning("item duplicated: " + _itemDescription[i]._id);
                    continue;
                }

                _scenarioScriptDictionary.Add(_itemDescription[i]._id, _itemDescription[i]._prefab);
            }

            _ready = true;
        }

        public void AddItem(GameObject newItem, string itemId)
        {

            OnItemAdded?.Invoke(itemId);
        }

        public GameObject GetItem(string index)
        {
            if (!ScenarioScriptDictionary.ContainsKey(index)) return null;

            return ScenarioScriptDictionary[index];
        }

        public GameObject[] GetItemArray() => ScenarioScriptList.ToArray();

        public void RemoveItem(string itemId)
        {
            if (!ScenarioScriptDictionary.ContainsKey(itemId)) return;

            var material = ScenarioScriptDictionary[itemId];
            for (int i = 0; i < _itemDescription.Count; i++)
            {
                if (_itemDescription[i]._id == itemId)
                {
                    _itemDescription.RemoveAt(i);
                    ScenarioScriptList.RemoveAt(i);
                }
            }

            ScenarioScriptDictionary.Remove(itemId);

            OnItemRemoved(itemId);

        }

        [Serializable]
        private class ItemDescription
        {
            [Uneditable]
            public string _id;

            public GameObject _prefab;
        }
    }
}
