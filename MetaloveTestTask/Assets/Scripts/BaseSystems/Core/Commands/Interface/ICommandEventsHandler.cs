using System;

namespace Scripts.BaseSystems.Core
{
    internal interface ICommandEventsHandler
    {
        public event Action<int, string> OnStopCommand;
        public event Action<int> OnClearCommandCategory;
        public event Action<ICommand> OnExecuteCommandImmediately;
        public event Action<ICommand> OnExecuteCommandThroughQueue;
        public event Action<int, string> OnCancelPreviousCommand;

        public event Action OnPrintCommandData;
        public event Action<int> OnCleareCommandHistoryByCategory;
        public event Action<string> OnCleareCommandHistoryFromClient;
        public event Action OnCleareCommandHistory;
    }
}
 