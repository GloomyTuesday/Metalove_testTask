using UnityEngine;
using TMPro;
using Scripts.BaseSystems;

namespace Scripts.ProjectSrc
{
    public class EditorObserver : MonoBehaviour
    {
        [SerializeField]
        private string _defaultScenarioFrameName; 

        [Space(15)]
        [SerializeField]
        private TextMeshProUGUI _applicationVerion;

        [Space(15)]
        [SerializeField]
        [FilterByType(typeof(IEditorEventsHandler))]
        private Object _editorEventsHandlerObj;

        [SerializeField]
        [FilterByType(typeof(IEditorEventsCallbackInvoker))]
        private Object _editorEventsCallbackInvokerObj;


        private IEditorEventsHandler _iEditorEventsHandler; 
        private IEditorEventsHandler IEditorEventsHandler
        {
            get
            {
                if(_iEditorEventsHandler == null)
                    _iEditorEventsHandler = _editorEventsHandlerObj.GetComponent<IEditorEventsHandler>();

                return _iEditorEventsHandler; 
            }
        }

        private IEditorEventsCallbackInvoker _iEditorEventsCallbackInvoker;
        private IEditorEventsCallbackInvoker IEditorEventsCallbackInvoker
        {
            get
            {
                if (_iEditorEventsCallbackInvoker == null)
                    _iEditorEventsCallbackInvoker = _editorEventsCallbackInvokerObj.GetComponent<IEditorEventsCallbackInvoker>();

                return _iEditorEventsCallbackInvoker;
            }
        }

        private void OnEnable()
        {
            Subscribe(); 
        }

        private void OnDisable()
        {
            Unsubscribe(); 
        }

        private void Subscribe()
        {
            IEditorEventsHandler.OnSaveCurrentScenarioFrame += SaveCurrentScenarioFrame;

            IEditorEventsHandler.OnLoadScenarioFrame += LoadScenarioFrame;
        }

        private void Unsubscribe()
        {
            IEditorEventsHandler.OnSaveCurrentScenarioFrame -= SaveCurrentScenarioFrame;

            IEditorEventsHandler.OnLoadScenarioFrame -= LoadScenarioFrame;
        }

        private void LoadScenarioFrame()
        {

        }

        private void SaveCurrentScenarioFrame()
        {
            
        }
    }
}
