using System;
using UnityEngine;

namespace Scripts.BaseSystems.MessageProcessor
{
    [CreateAssetMenu(menuName = "Scriptable Obj/Base systems/Core/Message processor/Message events")]
    public class MessageEventsSrc : ScriptableObject, IMessageEventsInvoker, IMessageEventsHandler
    {
        private Action<IMessage> _onSendRequest;
        event Action<IMessage> IMessageEventsHandler.OnSendRequest
        {
            add => _onSendRequest += value;
            remove => _onSendRequest -= value; 
        }
        void IMessageEventsInvoker.SendRequest(IMessage message) => _onSendRequest?.Invoke(message);


        private Action<IMessage> _onSendAnswer;
        event Action<IMessage> IMessageEventsHandler.OnSendAnswer
        {
            add => _onSendAnswer += value;
            remove => _onSendAnswer -= value;
        }
        void IMessageEventsInvoker.SendAnswer(IMessage message) => _onSendAnswer?.Invoke(message);


        private Action<IMessage> _onSendData;
        event Action<IMessage> IMessageEventsHandler.OnSendData
        {
            add => _onSendData += value;
            remove => _onSendData -= value;
        }
        void IMessageEventsInvoker.SendData(IMessage message) => _onSendData?.Invoke(message);


        private Action<IMessage> _onReceivedRequest;
        event Action<IMessage> IMessageEventsHandler.OnReceivedRequest
        {
            add => _onReceivedRequest += value;
            remove => _onReceivedRequest -= value;
        }
        void IMessageEventsInvoker.ReceivedRequest(IMessage message) => _onReceivedRequest?.Invoke(message);


        private Action<IMessage> _onReceivedAnswer;
        event Action<IMessage> IMessageEventsHandler.OnReceivedAnswer
        {
            add => _onReceivedAnswer += value;
            remove => _onReceivedAnswer -= value;
        }
        void IMessageEventsInvoker.ReceivedAnswer(IMessage message) => _onReceivedAnswer?.Invoke(message);


        private Action<IMessage> _onReceivedData;
        event Action<IMessage> IMessageEventsHandler.OnReceivedData
        {
            add => _onReceivedData += value;
            remove => _onReceivedData -= value;
        }
        void IMessageEventsInvoker.ReceivedData(IMessage message) => _onReceivedData?.Invoke(message);


        private Action<int, Action<IMessage>,bool> _onRegisterWaitingRequest;
        event Action<int, Action<IMessage>, bool> IMessageEventsHandler.OnRegisterWaitingRequest
        {
            add => _onRegisterWaitingRequest += value;
            remove => _onRegisterWaitingRequest -= value;
        }
        void IMessageEventsInvoker.RegisterWaitingRequest(
            int messageId, 
            Action<IMessage> messageProcessorAction,
            bool removeAfterProcessingComplete
            )=> _onRegisterWaitingRequest?.Invoke(messageId, messageProcessorAction, removeAfterProcessingComplete);


        private Action<int, Action<IMessage>, bool> _onRegisterWaitingAnswer;
        event Action<int, Action<IMessage>, bool> IMessageEventsHandler.OnRegisterWaitingAnswer
        {
            add => _onRegisterWaitingAnswer += value;
            remove => _onRegisterWaitingAnswer -= value;
        }
        void IMessageEventsInvoker.RegisterWaitingAnswer(
            int messageId,
            Action<IMessage> messageProcessorAction,
            bool removeAfterProcessingComplete
            ) => _onRegisterWaitingAnswer?.Invoke(messageId, messageProcessorAction, removeAfterProcessingComplete);


        private Action<int, Action<IMessage>, bool> _onRegisterWaitingData;
        event Action<int, Action<IMessage>, bool> IMessageEventsHandler.OnRegisterWaitingData
        {
            add => _onRegisterWaitingData += value;
            remove => _onRegisterWaitingData -= value;
        }
        void IMessageEventsInvoker.RegisterWaitingData(
            int messageId,
            Action<IMessage> messageProcessorAction,
            bool removeAfterProcessingComplete
            ) => _onRegisterWaitingData?.Invoke(messageId, messageProcessorAction, removeAfterProcessingComplete);


        private Action<int, Action<IMessage>> _onRemoveWaitingRequest;
        event Action<int, Action<IMessage>> IMessageEventsHandler.OnRemoveWaitingRequest
        {
            add => _onRemoveWaitingRequest += value;
            remove => _onRemoveWaitingRequest -= value;
        }
        void IMessageEventsInvoker.RemoveWaitingRequest(
            int messageId,
            Action<IMessage> messageProcessorAction
            ) => _onRemoveWaitingRequest?.Invoke(messageId, messageProcessorAction);


        private Action<int, Action<IMessage>> _onRemoveWaitingAnswer;
        event Action<int, Action<IMessage>> IMessageEventsHandler.OnRemoveWaitingAnswer
        {
            add => _onRemoveWaitingAnswer += value;
            remove => _onRemoveWaitingAnswer -= value;
        }
        void IMessageEventsInvoker.RemoveWaitingAnswer(
            int messageId,
            Action<IMessage> messageProcessorAction
            ) => _onRemoveWaitingAnswer?.Invoke(messageId, messageProcessorAction);


        private Action<int, Action<IMessage>> _onRemoveWaitingData;
        event Action<int, Action<IMessage>> IMessageEventsHandler.OnRemoveWaitingData
        {
            add => _onRemoveWaitingData += value;
            remove => _onRemoveWaitingData -= value;
        }
        void IMessageEventsInvoker.RemoveWaitingData(
            int messageId,
            Action<IMessage> messageProcessorAction
            ) => _onRemoveWaitingData?.Invoke(messageId, messageProcessorAction);
    }
}
