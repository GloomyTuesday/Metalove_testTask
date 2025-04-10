using System;

namespace Scripts.BaseSystems.MessageProcessor
{
    public interface IMessageEventsInvoker 
    {
        public void SendRequest(IMessage message);
        public void SendAnswer(IMessage message);
        public void SendData(IMessage message);

        public void ReceivedRequest(IMessage message);
        public void ReceivedAnswer(IMessage message);
        public void ReceivedData(IMessage message);

        public void RegisterWaitingRequest(
            int messageId,
            Action<IMessage> messageProcessorAction,
            bool removeAfterProcessingComplete = false
            );
        public void RegisterWaitingAnswer(
            int messageId,
            Action<IMessage> messageProcessorAction,
            bool removeAfterProcessingComplete = false
            );
        public void RegisterWaitingData(
            int messageId,
            Action<IMessage> messageProcessorAction,
            bool removeAfterProcessingComplete = false
            );

        public void RemoveWaitingRequest(int messageId,Action<IMessage> messageProcessorAction);
        public void RemoveWaitingData(int messageId, Action<IMessage> messageProcessorAction);
        public void RemoveWaitingAnswer(int messageId, Action<IMessage> messageProcessorAction);
    }
}
