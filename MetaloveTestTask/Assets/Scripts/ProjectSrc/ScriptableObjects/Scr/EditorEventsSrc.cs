using Scripts.ProjectSrc;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "EditorEvents", menuName = "Scriptable Obj/Project src/Editor/Editor events")]
public class EditorEventsSrc : ScriptableObject,
    IEditorEventsInvoker,
    IEditorEventsHandler,
    IEditorEventsCallbackInvoker,
    IEditorEventsCallbackHandler
{

    private Action _onSaveCurrentScenarioFrame; 
    event Action IEditorEventsHandler.OnSaveCurrentScenarioFrame
    {
        add => _onSaveCurrentScenarioFrame += value; 
        remove => _onSaveCurrentScenarioFrame -= value;
    }
    public void SaveCurrentScenarioFrame() => _onSaveCurrentScenarioFrame?.Invoke();


    private Action<string> _onSetScenarioFrameId;
    event Action<string> IEditorEventsHandler.OnSetScenarioFrameId
    {
        add => _onSetScenarioFrameId += value;
        remove => _onSetScenarioFrameId -= value;
    }
    public void SetScenarioFrameId(string id) => _onSetScenarioFrameId?.Invoke(id);


    private Action _onLoadScenarioFrame;
    event Action IEditorEventsHandler.OnLoadScenarioFrame
    {
        add => _onLoadScenarioFrame += value;
        remove => _onLoadScenarioFrame -= value;
    }
    public void LoadScenarioFrame() => _onLoadScenarioFrame?.Invoke();
}
