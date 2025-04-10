using Scripts.ProjectSrc;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ScenarioFrameEventsSrc", menuName = "Scriptable Obj/Project src/ScenarioFrameEventsSrc")]
public class ScenarioFrameEventsSrc : ScriptableObject, IScenarioFrameEvents
{
    public event Action OnLoadPreviousScenarioFrameId;
    public void LoadPreviousScenarioFrameId() => OnLoadPreviousScenarioFrameId?.Invoke(); 

    public event Action<string> OnLoadScenarioFrame;
    public void LoadScenarioFrame(string scenarioFrameId) => OnLoadScenarioFrame?.Invoke(scenarioFrameId); 


    public event Action<string, AlignmentId> OnApplyCharacter;
    public void ApplyCharacter(string characterId, AlignmentId alignmentId) => OnApplyCharacter?.Invoke(characterId, alignmentId); 


    public event Action<string[], string[]> OnApplyChoices;
    public void ApplyChoices(string[] choicesText, string[] choicesScriptFramId) => OnApplyChoices?.Invoke(choicesText, choicesScriptFramId); 


    public event Action<string> OnApplyText;
    public void ApplyText(string text) => OnApplyText?.Invoke(text);


    public event Action<string> OnApplyNextScenarioFrameToLoad;
    public void ApplyNextScenarioFrameToLoad(string scenarioFrameId) => OnApplyNextScenarioFrameToLoad?.Invoke(scenarioFrameId);


    public event Func<string> OnGetNextScenarioFrameId;
    public string GetNextScenarioFrameId() => OnGetNextScenarioFrameId?.Invoke();


    public event Func<(string[] choicesText, string[] choiceNextFrameId)> OnGetScenarioFrameChoicesData;
    public (string[], string[]) GetScenarioFrameChoicesData()
    {
        var result = OnGetScenarioFrameChoicesData?.Invoke();

        if (result == null) return (null, null);

        return (result.Value.choicesText, result.Value.choiceNextFrameId);
    } 
}
