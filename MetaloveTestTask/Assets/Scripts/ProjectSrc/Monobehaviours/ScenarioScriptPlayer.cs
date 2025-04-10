using Scripts.BaseSystems;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.ProjectSrc
{
    public class ScenarioScriptPlayer : MonoBehaviour
    {
        [Space(15)]
        [SerializeField]
        [FilterByType(typeof(IBankTypeId<string, TextAsset>))]
        private UnityEngine.Object _scriptsFrameObj;

        [SerializeField]
        [FilterByType(typeof(IScenarioFrameEvents))]
        private UnityEngine.Object _scenarioFrameEventsObj;

        [SerializeField]
        [FilterByType(typeof(IScenarioFrameToLoadBuffer))]
        private UnityEngine.Object _scenarioFrameToLoadBufferObj;

        private Stack<string> ScenarioFrameStack { get; set; } = new Stack<string>();

        private ScenarioFrameModel _scenarioFrameModel;
        private ScenarioFrameModel ScenarioFrameModel { get => _scenarioFrameModel; set => _scenarioFrameModel = value; }

        private IBankTypeId<string, TextAsset> _iScenarioFrameBank;
        private IBankTypeId<string, TextAsset> IScenarioFrameBank
        {
            get
            {
                if (_iScenarioFrameBank == null)
                    _iScenarioFrameBank = _scriptsFrameObj.GetComponent<IBankTypeId<string, TextAsset>>();

                return _iScenarioFrameBank;
            }
        }

        private IScenarioFrameEvents _iScenarioFrameEvents;
        private IScenarioFrameEvents IScenarioFrameEvents
        {
            get
            {
                if (_iScenarioFrameEvents == null)
                    _iScenarioFrameEvents = _scenarioFrameEventsObj.GetComponent<IScenarioFrameEvents>();

                return _iScenarioFrameEvents;
            }
        }

        private IScenarioFrameToLoadBuffer _iScenarioFrameToLoadBuffer; 
        private IScenarioFrameToLoadBuffer IScenarioFrameToLoadBuffer
        {
            get
            {
                if(_iScenarioFrameToLoadBuffer == null)
                    _iScenarioFrameToLoadBuffer = _scenarioFrameToLoadBufferObj.GetComponent<IScenarioFrameToLoadBuffer>();

                return _iScenarioFrameToLoadBuffer;
            }
        }

        private string CurrentScenarioFrame { get; set; }

        private void OnEnable()
        {
            ScenarioFrameStack = new Stack<string>();

            Subscribe(); 

            if (string.IsNullOrEmpty(IScenarioFrameToLoadBuffer.ScenarioFrameId)) return;

            StartCoroutine(Co_InitWithDelay()); 
        }

        private void OnDisable()
        {
            Unsubscribe(); 
        }

        private void Subscribe()
        {
            IScenarioFrameEvents.OnLoadPreviousScenarioFrameId += LoadPreviousScenarioFrameId;
            IScenarioFrameEvents.OnLoadScenarioFrame += LoadScenarioFrameAndEnqueu;
            IScenarioFrameEvents.OnGetScenarioFrameChoicesData += GetScenarioFrameChoicesData;
            IScenarioFrameEvents.OnGetNextScenarioFrameId += GetNextScenarioFrameId;
        }

        private void Unsubscribe()
        {
            IScenarioFrameEvents.OnLoadPreviousScenarioFrameId -= LoadPreviousScenarioFrameId;
            IScenarioFrameEvents.OnLoadScenarioFrame -= LoadScenarioFrameAndEnqueu;
            IScenarioFrameEvents.OnGetScenarioFrameChoicesData -= GetScenarioFrameChoicesData;
            IScenarioFrameEvents.OnGetNextScenarioFrameId -= GetNextScenarioFrameId;
        }

        private (string[] choicesText, string[] choiceNextFrameId) GetScenarioFrameChoicesData()
        {
            return (ScenarioFrameModel._choiceOptionText, ScenarioFrameModel._choiceOptionScenarioFrameId); 
        }

        private string GetNextScenarioFrameId() => ScenarioFrameModel._nextScenarioFrameId;

        private IEnumerator Co_InitWithDelay()
        {
            yield return new WaitForEndOfFrame();

            var scenarioFrameId = IScenarioFrameToLoadBuffer.ScenarioFrameId;
            LoadScenarioFrame(scenarioFrameId, true); 
        }

        private void TryToEnqueu(string scenarioFrameId)
        {
            if (string.IsNullOrEmpty(scenarioFrameId)) return; 

            if(ScenarioFrameStack.Count<1)
            {
                ScenarioFrameStack.Push(scenarioFrameId);
                return; 
            }

            if (ScenarioFrameStack.Peek().Equals(scenarioFrameId)) return;

            ScenarioFrameStack.Push(scenarioFrameId);
        }

        private void LoadScenarioFrameAndEnqueu(string scenarioFrameId)
        {
            LoadScenarioFrame(scenarioFrameId, true);
        }

        private void LoadScenarioFrame(string scenarioFrameId , bool pushScenarioFrameToStack)
        {
            var scenarioFrameTextAsset = IScenarioFrameBank.GetItem(scenarioFrameId);

            if (scenarioFrameTextAsset == null) return; 

            try
            {
                ScenarioFrameModel = JsonUtility.FromJson<ScenarioFrameModel>(scenarioFrameTextAsset.text);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
                return; 
            }

            if (pushScenarioFrameToStack)
                ScenarioFrameStack.Push(CurrentScenarioFrame);

            CurrentScenarioFrame = scenarioFrameId;

            IScenarioFrameEvents.ApplyText(ScenarioFrameModel._text);

            if (ScenarioFrameModel._choiceOptionText != null && ScenarioFrameModel._choiceOptionText.Length > 0)
                IScenarioFrameEvents.ApplyNextScenarioFrameToLoad("");
           
            IScenarioFrameEvents.ApplyChoices(ScenarioFrameModel._choiceOptionText, ScenarioFrameModel._choiceOptionScenarioFrameId); IScenarioFrameEvents.ApplyNextScenarioFrameToLoad(ScenarioFrameModel._nextScenarioFrameId);

            if (!string.IsNullOrEmpty(ScenarioFrameModel._characterId))
            {
                IScenarioFrameEvents.ApplyCharacter(ScenarioFrameModel._characterId, ScenarioFrameModel._characterAlignment);
            }
        }

        private void LoadPreviousScenarioFrameId()
        {
            if (ScenarioFrameStack.Count < 1 ) return;

            var qeueScenarioFrame = ScenarioFrameStack.Pop();

            while (CurrentScenarioFrame.Equals(qeueScenarioFrame) && ScenarioFrameStack.Count>0)
                qeueScenarioFrame = ScenarioFrameStack.Pop();

            if (string.IsNullOrEmpty(qeueScenarioFrame)) return; 
            LoadScenarioFrame(qeueScenarioFrame, false); 
        }
    }
}
