using System;

namespace Scripts.ProjectSrc
{
    public interface IEditorEventsHandler
    {
        public event Action OnSaveCurrentScenarioFrame;

        public event Action<string> OnSetScenarioFrameId;

        public event Action OnLoadScenarioFrame;
    }
}
