using UnityEngine;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Scripts.BaseSystems.Core
{
    public partial class CommandObserver : MonoBehaviour, IReady
    {

        const int COMMAND_QUEUE_SIZE = 2;

        [SerializeField, FilterByType(typeof(ICommandEventsHandler))]
        private Object _commandEventsHandlerObj;

        //  As key is command category id, for best practice should be used an Enum casted to int
        //  key for second dictionary is an int value used as player id or other name that describe client that launched that command.
        //  And list should not be bigger than: COMMAND_QUEUE_SIZE
        private Dictionary<int, Dictionary<string, CommandProcessorDataUnit>> CommandProcessorDictionary { get; set; } =
            new Dictionary<int, Dictionary<string, CommandProcessorDataUnit>>();

        private Dictionary<int, Dictionary<string, Stack<ICommand>>> CompletedCommandRepositoryDictionary { get; set; } =
            new Dictionary<int, Dictionary<string, Stack<ICommand>>>();

        private ICommandEventsHandler _iCommandEventsHandler;
        private ICommandEventsHandler ICommandEventsHandler
        {
            get
            {
                if (_iCommandEventsHandler == null)
                    _iCommandEventsHandler = _commandEventsHandlerObj.GetComponent<ICommandEventsHandler>();

                return _iCommandEventsHandler; 
            }
        }

        public bool Ready { get; set; }

        private class CommandProcessorDataUnit
        {
            public CancellationTokenSource CancellationTokenSource { get; set; } = default;
            public List<ICommand> CommandQueueList { get; set; } = new List<ICommand>(COMMAND_QUEUE_SIZE);
        }

        private void OnEnable()
        {
            Subscribe();
            Ready = true; 
        }

        private void OnDisable()
        {
            Unsubscribe();
        }

        private void Subscribe()
        {
            ICommandEventsHandler.OnExecuteCommandThroughQueue += ExecuteCommandThroughQueue;
            ICommandEventsHandler.OnExecuteCommandImmediately += ExecuteCommandImmediately;
            ICommandEventsHandler.OnClearCommandCategory += ClearCommandCategory;
            ICommandEventsHandler.OnCancelPreviousCommand += CancelPreviousCommand;

            ICommandEventsHandler.OnPrintCommandData += PrintCommandData;
            ICommandEventsHandler.OnCleareCommandHistoryFromClient += CleareCommandHistoryFromClient;
            ICommandEventsHandler.OnCleareCommandHistoryByCategory += CleareCommandHistoryByCategory;
            ICommandEventsHandler.OnCleareCommandHistory += CleareCommandHistory;
        }

        private void CancelPreviousCommand(int commandCategory, string clientId)
        {
            if (!CompletedCommandRepositoryDictionary.ContainsKey(commandCategory)) return;
            if (!CompletedCommandRepositoryDictionary[commandCategory].ContainsKey(clientId)) return;
            if (CompletedCommandRepositoryDictionary[commandCategory][clientId].Count < 1) return;

            var commandToCancel = CompletedCommandRepositoryDictionary[commandCategory][clientId].Pop();
            var cancelNext = commandToCancel.CancelNextCommandAsWell;
            commandToCancel.Cancel();

            if (cancelNext)
                CancelPreviousCommand(commandCategory, clientId);
        }

        private void Unsubscribe()
        {
            ICommandEventsHandler.OnExecuteCommandThroughQueue -= ExecuteCommandThroughQueue;
            ICommandEventsHandler.OnExecuteCommandImmediately -= ExecuteCommandImmediately;
            ICommandEventsHandler.OnClearCommandCategory -= ClearCommandCategory;
            ICommandEventsHandler.OnCancelPreviousCommand -= CancelPreviousCommand;

            ICommandEventsHandler.OnPrintCommandData -= PrintCommandData;
            ICommandEventsHandler.OnCleareCommandHistoryFromClient -= CleareCommandHistoryFromClient;
            ICommandEventsHandler.OnCleareCommandHistoryByCategory -= CleareCommandHistoryByCategory;
            ICommandEventsHandler.OnCleareCommandHistory -= CleareCommandHistory;
        }

        private void PrintCommandData()
        {
            //  CompletedCommandRepositoryDictionary
            Debug.Log("\t - CommandObserver \t PrintCommandData() ");
            Debug.Log("\t\t "+ COMMAND_QUEUE_SIZE);
            Debug.Log(" ");
            Debug.Log("\t\t Completed command repository data: ");
            Debug.Log("\t\t\t ");

            int counter = 0; 

            foreach (var commandCategory in CompletedCommandRepositoryDictionary)
            {
                Debug.Log("\t\t\t Category: "+ commandCategory.Key);
                Debug.Log(" " );

                foreach (var clientDictionary in commandCategory.Value)
                {
                    Debug.Log("\t\t\t\t Client id: "+ clientDictionary.Key);
                    foreach (var item in clientDictionary.Value)
                    {
                        Debug.Log("\t\t\t\t\t [ "+ counter + " ] " + item.ToString()+"\t "+item.CancelNextCommandAsWell);
                        counter++;
                    }
                    counter = 0; 
                    Debug.Log(" ");
                }
                Debug.Log(" ");
            }
        }

        private void CleareCommandHistoryFromClient(string clientId)
        {
            List<int> listWithCategoryId = new List<int>(); 

            foreach (var item in CompletedCommandRepositoryDictionary)
            {
                if (!item.Value.ContainsKey(clientId)) continue;

                listWithCategoryId.Add(item.Key); 
            }

            for (int i = 0; i < listWithCategoryId.Count; i++)
            {
                if (!CompletedCommandRepositoryDictionary[listWithCategoryId[i]].ContainsKey( clientId )) continue;

                CompletedCommandRepositoryDictionary[listWithCategoryId[i]].Remove(clientId); 
            }
        }

        private void CleareCommandHistoryByCategory(int obj)
        {
            if (!CompletedCommandRepositoryDictionary.ContainsKey(obj)) return;
                CompletedCommandRepositoryDictionary.Remove(obj);
        }

        private void CleareCommandHistory()
        {
            CompletedCommandRepositoryDictionary.Clear();
        }

        private async void ExecuteCommandImmediately(ICommand command)
        {
            //  Debug.Log("\t - CommandObserver \t ExecuteCommandImmediately \t " + command.ToString() + "\t " + command.CancelNextCommandAsWell);
            CompleteDictionaryForMissingKeys(command.CategoryId, command.ClientId);
            StopCurrentCommandIfExecuting(command.CategoryId, command.ClientId);
            await LaunchCommandExecution(command);
        }

        private async void ExecuteCommandThroughQueue(ICommand command)
        {
            //  Debug.Log("\t - CommandObserver \t ExecuteCommandThroughQueue \t " + command.ToString() + "\t " + command.CancelNextCommandAsWell);
            var categoryId = command.CategoryId;
            var clientId = command.ClientId;
            CompleteDictionaryForMissingKeys(categoryId, clientId);

            if (CommandProcessorDictionary[categoryId][clientId].CommandQueueList.Count > 0)
            {
                if (CommandProcessorDictionary[categoryId][clientId].CommandQueueList.Count >= COMMAND_QUEUE_SIZE)
                    CommandProcessorDictionary[categoryId][clientId].CommandQueueList[COMMAND_QUEUE_SIZE - 1] = command;
                else
                    CommandProcessorDictionary[categoryId][clientId].CommandQueueList.Add(command);

                return;
            }
            
            await LaunchCommandExecution(command);
        }

        private void CompleteDictionaryForMissingKeys(int categoryId, string clientId)
        {
            if (!CommandProcessorDictionary.ContainsKey(categoryId))
            {
                CommandProcessorDictionary.Add(categoryId, new Dictionary<string, CommandProcessorDataUnit>());
                CompletedCommandRepositoryDictionary.Add(categoryId, new Dictionary<string, Stack<ICommand>>());
            }


            if (!CommandProcessorDictionary[categoryId].ContainsKey(clientId))
                CommandProcessorDictionary[categoryId].Add(clientId, new CommandProcessorDataUnit());
        }

        private void StopCurrentCommandIfExecuting(int commandCategoryId, string clientId, bool clearCommandQueue = true)
        {
            if (!CommandProcessorDictionary.ContainsKey(commandCategoryId)) return;
            if (!CommandProcessorDictionary[commandCategoryId].ContainsKey(clientId)) return;
            if (CommandProcessorDictionary[commandCategoryId][clientId].CancellationTokenSource == default) return;

            CommandProcessorDictionary[commandCategoryId][clientId].CancellationTokenSource.Cancel();

            if (clearCommandQueue)
                CommandProcessorDictionary[commandCategoryId][clientId].CommandQueueList.Clear();
        }

        private async Task LaunchCommandExecution(ICommand command)
        {
            var categoryId = command.CategoryId;
            var clientId = command.ClientId;
            CommandProcessorDictionary[categoryId][clientId].CommandQueueList.Add(command);

            while (CommandProcessorDictionary[categoryId][clientId].CommandQueueList.Count > 0)
            {
                CommandProcessorDictionary[categoryId][clientId].CancellationTokenSource = new CancellationTokenSource();
                var token = CommandProcessorDictionary[categoryId][clientId].CancellationTokenSource.Token;

                await CommandProcessorDictionary[categoryId][clientId].CommandQueueList[0].Execute(token);

                AddCompletedCommandToRepository(CommandProcessorDictionary[categoryId][clientId].CommandQueueList[0]);
                //  _commandEvents.AddCompletedCommandToRepository(CommandProcessorDictionary[categoryId][clientId].CommandQueueList[0]);
                CommandProcessorDictionary[categoryId][clientId].CommandQueueList.RemoveAt(0);

                if (CommandProcessorDictionary[categoryId][clientId].CommandQueueList.Count < 1) break;
            }

            CommandProcessorDictionary[categoryId][clientId].CancellationTokenSource = default;
        }

        private void ClearCommandCategory(int commandCategoryId)
        {
            if (!CommandProcessorDictionary.ContainsKey(commandCategoryId)) return;

            foreach (var item in CommandProcessorDictionary[commandCategoryId])
                StopCurrentCommandIfExecuting(commandCategoryId, item.Key);

            CommandProcessorDictionary.Remove(commandCategoryId);
            CompletedCommandRepositoryDictionary.Remove(commandCategoryId); 
        }

        private void AddCompletedCommandToRepository(ICommand command)
        {
            var categoryId = command.CategoryId;
            var clientId = command.ClientId;
            if (!CompletedCommandRepositoryDictionary.ContainsKey(command.CategoryId))
                CompletedCommandRepositoryDictionary.Add(categoryId, new Dictionary<string, Stack<ICommand>>());

            if (!CompletedCommandRepositoryDictionary[categoryId].ContainsKey(clientId))
                CompletedCommandRepositoryDictionary[categoryId].Add(clientId, new Stack<ICommand>());

            CompletedCommandRepositoryDictionary[categoryId][clientId].Push(command);
        }
    }
}
