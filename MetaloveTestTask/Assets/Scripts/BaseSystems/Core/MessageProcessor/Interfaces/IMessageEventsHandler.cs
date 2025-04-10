using System;

namespace Scripts.BaseSystems.MessageProcessor
{
    public interface IMessageEventsHandler 
    {
        public event Action<IMessage> OnSendRequest;
        public event Action<IMessage> OnSendAnswer;
        public event Action<IMessage> OnSendData;

        public event Action<IMessage> OnReceivedRequest;
        public event Action<IMessage> OnReceivedAnswer;
        public event Action<IMessage> OnReceivedData;

        public event Action<int, Action<IMessage>, bool> OnRegisterWaitingRequest;
        public event Action<int, Action<IMessage>, bool> OnRegisterWaitingAnswer;
        public event Action<int, Action<IMessage>, bool> OnRegisterWaitingData;

        public event Action<int, Action<IMessage>> OnRemoveWaitingRequest;
        public event Action<int, Action<IMessage>> OnRemoveWaitingAnswer;
        public event Action<int, Action<IMessage>> OnRemoveWaitingData;
    }
}
