using System;

namespace Scripts.ProjectSrc
{
    public interface IScenarioFrameEvents
    {

        public event Action<string> OnLoadScenarioFrame;
        public void LoadScenarioFrame(string scenarioFrameId);
        

        public event Action<string> OnApplyText;
        public void ApplyText(string text);


        public event Action<string> OnApplyNextScenarioFrameToLoad; 
        public void ApplyNextScenarioFrameToLoad(string scenarioFrameId);


        public event Action<string[], string[]> OnApplyChoices;
        public void ApplyChoices(string[] choicesText, string[] choicesScriptFramId);
        

        public event Action<string, AlignmentId> OnApplyCharacter;
        public void ApplyCharacter(string characterId, AlignmentId alignmentId);
        

        public event Func<string> OnGetNextScenarioFrameId;
        public string GetNextScenarioFrameId();


        public event Func<(string[] choicesText, string[] choiceNextFrameId)> OnGetScenarioFrameChoicesData;
        public (string[], string[]) GetScenarioFrameChoicesData();


        public event Action OnLoadPreviousScenarioFrameId;
        public void LoadPreviousScenarioFrameId();
    }
}
