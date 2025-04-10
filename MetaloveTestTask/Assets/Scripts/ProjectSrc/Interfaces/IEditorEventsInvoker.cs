namespace Scripts.ProjectSrc
{
    public interface IEditorEventsInvoker
    {
        public void SaveCurrentScenarioFrame();

        public void SetScenarioFrameId(string id);

        public void LoadScenarioFrame();
    }
}
