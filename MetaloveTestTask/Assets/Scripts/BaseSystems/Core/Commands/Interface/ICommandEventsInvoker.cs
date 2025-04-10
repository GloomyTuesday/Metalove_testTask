namespace Scripts.BaseSystems.Core
{
    public interface ICommandEventsInvoker
    {
        public void StopCommand(int commandId, string clientId);
        public void ClearCommandCategory(int commandCategory);
        public void ExecuteCommandImmediately(ICommand command);
        public void ExecuteCommand(ICommand command);
        public void CancelPreviousCommand(int commandcategory, string clientId);

        public void PrintCommandData();
        public void CleareCommandHistoryFromClient(string clientId);
        public void CleareCommandHistoryByCategory(int categoryId);
        public void CleareCommandHistory();
    }
}
