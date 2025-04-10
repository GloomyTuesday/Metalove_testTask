using UnityEngine;
using System;

namespace Scripts.BaseSystems.Core
{
    [CreateAssetMenu(fileName = "CommandEvents", menuName = "Scriptable Obj/Base systems/Core/Command/Command events")]
    public class CommandEventsSrc : ScriptableObject, ICommandEventsInvoker, ICommandEventsHandler
    {
        
        private Action<int, string> _OnStopCommand;
        event Action<int, string> ICommandEventsHandler.OnStopCommand
        {
            add => _OnStopCommand = value;
            remove => _OnStopCommand = value; 
        }
        public void StopCommand(int commandId, string clientId) => _OnStopCommand?.Invoke(commandId , clientId);


        private Action<int> _OnClearCommandCategory;
        event Action<int> ICommandEventsHandler.OnClearCommandCategory
        {
            add => _OnClearCommandCategory = value;
            remove => _OnClearCommandCategory = value; 
        }
        public void ClearCommandCategory(int commandCategory) => _OnClearCommandCategory?.Invoke(commandCategory);


        private Action<ICommand> _OnExecuteCommandImmediately;
        event Action<ICommand> ICommandEventsHandler.OnExecuteCommandImmediately
        {
            add => _OnExecuteCommandImmediately = value; 
            remove => _OnExecuteCommandImmediately = value; 
        }
        public void ExecuteCommandImmediately(ICommand command) => _OnExecuteCommandImmediately?.Invoke(command);


        private Action<ICommand> _OnExecuteCommandThroughQueue;
        event Action<ICommand> ICommandEventsHandler.OnExecuteCommandThroughQueue
        {
            add => _OnExecuteCommandThroughQueue = value;
            remove => _OnExecuteCommandThroughQueue = value; 
        }
        public void ExecuteCommand(ICommand command) => _OnExecuteCommandThroughQueue?.Invoke(command);


        private Action<int, string> _OnCancelPreviousCommand;
        event Action<int, string> ICommandEventsHandler.OnCancelPreviousCommand
        {
            add => _OnCancelPreviousCommand = value;
            remove => _OnCancelPreviousCommand = value; 
        }
        public void CancelPreviousCommand(int commandcategory, string clientId) =>
            _OnCancelPreviousCommand?.Invoke(commandcategory, clientId);


        private Action _onPrintCommandData; 
        event Action ICommandEventsHandler.OnPrintCommandData
        {
            add => _onPrintCommandData = value;
            remove => _onPrintCommandData = value;
        }
        public void PrintCommandData() => _onPrintCommandData?.Invoke();


        private Action<string> _onCleareCommandHistoryFromClient;
        event Action<string> ICommandEventsHandler.OnCleareCommandHistoryFromClient
        {
            add => _onCleareCommandHistoryFromClient = value;
            remove => _onCleareCommandHistoryFromClient = value;
        }
        public void CleareCommandHistoryFromClient(string clientId ) => _onCleareCommandHistoryFromClient?.Invoke(clientId);


        private Action<int> _onCleareCommandHistoryByCategory;
        event Action<int> ICommandEventsHandler.OnCleareCommandHistoryByCategory
        {
            add => _onCleareCommandHistoryByCategory = value;
            remove => _onCleareCommandHistoryByCategory = value;
        }
        public void CleareCommandHistoryByCategory(int categoryId) => _onCleareCommandHistoryByCategory?.Invoke(categoryId);


        private Action _onCleareCommandHistory;
        event Action ICommandEventsHandler.OnCleareCommandHistory
        {
            add => _onCleareCommandHistory = value;
            remove => _onCleareCommandHistory = value;
        }
        public void CleareCommandHistory() => _onCleareCommandHistory?.Invoke();
    }
}
