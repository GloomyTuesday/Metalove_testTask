using Scripts.BaseSystems;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.ProjectSrc
{

    [CreateAssetMenu(fileName = "ScenarioBank", menuName = "Scriptable Obj/Project src/Scenario bank")]
    public class ScenarioBankSrc : ScriptableObject, IBankTypeId<string, TextAsset>
    {
        [SerializeField]
        private List<ItemDescription> _scenarioFrame;

        [NonSerialized]
        private bool _ready;

        private List<TextAsset> _scenarioScriptList;
        public List<TextAsset> ScenarioScriptList
        {
            get
            {
                if (!_ready)
                    Init();

                return _scenarioScriptList;
            }
        }

        private Dictionary<string, TextAsset> _scenarioScriptDictionary;
        private Dictionary<string, TextAsset> ScenarioScriptDictionary
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
            if (_scenarioFrame == null) return;

            for (int i = 0; i < _scenarioFrame.Count; i++)
            {
                _scenarioFrame[i]._id = _scenarioFrame[i]._jsonFile.name; 
            }
        }

        private void Init()
        {
            _scenarioScriptDictionary = new Dictionary<string, TextAsset>();
            _scenarioScriptList = new List<TextAsset>();

            for (int i = 0; i < _scenarioFrame.Count; i++)
            {
                _scenarioScriptList.Add(_scenarioFrame[i]._jsonFile);

                if (_scenarioScriptDictionary.ContainsKey(_scenarioFrame[i]._id))
                {
                    Debug.LogWarning("item duplicated: " + _scenarioFrame[i]._id);
                    continue;
                }

                _scenarioScriptDictionary.Add(_scenarioFrame[i]._id, _scenarioFrame[i]._jsonFile);
            }

            _ready = true;
        }

        public void AddItem(TextAsset newItem, string itemId)
        {

            OnItemAdded?.Invoke(itemId); 
        }

        public TextAsset GetItem(string index)
        {
            if (!ScenarioScriptDictionary.ContainsKey(index)) return null; 

            return ScenarioScriptDictionary[index];
        }

        public TextAsset[] GetItemArray() => ScenarioScriptList.ToArray(); 

        public void RemoveItem(string itemId)
        {
            if (!ScenarioScriptDictionary.ContainsKey(itemId)) return;

            var material = ScenarioScriptDictionary[itemId];
            for (int i = 0; i < _scenarioFrame.Count; i++)
            {
                if (_scenarioFrame[i]._id == itemId)
                {
                    _scenarioFrame.RemoveAt(i);
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
            public TextAsset _jsonFile;
        }
    }
}
