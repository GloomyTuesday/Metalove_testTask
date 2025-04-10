using Scripts.BaseSystems.NetworkBase;
using System;
using UnityEngine;

namespace Scripts.BaseSystems.MessageProcessor
{
    public class MessageProcessorObserver : MonoBehaviour
    {
        [SerializeField, FilterByType(typeof(IMessageEventsInvoker))]
        private UnityEngine.Object _messageEventsInvokerObj;
        [SerializeField, FilterByType(typeof(IMessageEventsHandler))]
        private UnityEngine.Object _messageEventsHandlerObj;
        [SerializeField, FilterByType(typeof(INetworkCallbackHandler))]
        private UnityEngine.Object _networkApiEmitedEventsObj;
        [SerializeField, FilterByType(typeof(INetworkEventsInvoker))]
        private UnityEngine.Object _networkApiReceiverEventsInvokerObj;

        private bool IsNetworkConnected { get; set; }
        private bool IsInTheNetworkRoom { get; set; }

        //  ----------------------------------------    Instances
        #region Instances
        private INetworkCallbackHandler _iNetworkApiEmitedEvents;
        private INetworkCallbackHandler INetworkApiEmitedEvents
        {
            get
            {
                if (_iNetworkApiEmitedEvents == null)
                    _iNetworkApiEmitedEvents = _networkApiEmitedEventsObj.GetComponent<INetworkCallbackHandler>();

                return _iNetworkApiEmitedEvents;
            }
        }

        private INetworkEventsInvoker _iNetworkApiReceiverEventsInvoker;
        private INetworkEventsInvoker INetworkApiReceiverEventsInvoker
        {
            get
            {
                if (_iNetworkApiReceiverEventsInvoker == null)
                    _iNetworkApiReceiverEventsInvoker = _networkApiReceiverEventsInvokerObj.GetComponent<INetworkEventsInvoker>();

                return _iNetworkApiReceiverEventsInvoker;
            }
        }

        private IMessageEventsInvoker _iMessageEventsInvoker;
        private IMessageEventsInvoker IMessageEventsInvoker
        {
            get
            {
                if (_iMessageEventsInvoker == null)
                    _iMessageEventsInvoker = _messageEventsInvokerObj.GetComponent<IMessageEventsInvoker>();

                return _iMessageEventsInvoker;
            }
        }

        private IMessageEventsHandler _iMessageEventsHandler;
        private IMessageEventsHandler IMessageEventsHandler
        {
            get
            {
                if (_iMessageEventsHandler == null)
                    _iMessageEventsHandler = _messageEventsInvokerObj.GetComponent<IMessageEventsHandler>();

                return _iMessageEventsHandler;
            }
        }
        #endregion

        private void OnEnable()
        {
            Subscribe();
        }

        private void OnDisable()
        {
            Unsubscribe();
        }

        private void Subscribe()
        {
            INetworkApiEmitedEvents.OnLeaveRoomOccured += LeaveRoomOccured;
            INetworkApiEmitedEvents.OnJoinRoomOccured += JoinRoomOccured;
            INetworkApiEmitedEvents.OnConnectionLost += ConnectionLost;
            INetworkApiEmitedEvents.OnConnectionOccured += ConnectionOccured;
            INetworkApiEmitedEvents.OnReceivedNetworkMessage += ReceivedNetworkMessage;

            IMessageEventsHandler.OnSendRequest += SendRequest;
            IMessageEventsHandler.OnSendAnswer += SendAnswer;
            IMessageEventsHandler.OnSendData += SendData;

            IMessageEventsHandler.OnReceivedRequest += ReceivedRequest;
            IMessageEventsHandler.OnReceivedAnswer += ReceivedAnswer;
            IMessageEventsHandler.OnReceivedData += ReceivedData;

            IMessageEventsHandler.OnRegisterWaitingRequest += RegisterWaitingRequest;
            IMessageEventsHandler.OnRegisterWaitingAnswer += RegisterWaitingAnswer;
            IMessageEventsHandler.OnRegisterWaitingData += RegisterWaitingData;

            IMessageEventsHandler.OnRemoveWaitingRequest += RemoveWaitingRequest;
            IMessageEventsHandler.OnRemoveWaitingAnswer += RemoveWaitingAnswer;
            IMessageEventsHandler.OnRemoveWaitingData += RemoveWaitingData;

        }

        private void LeaveRoomOccured()
        {
            IsInTheNetworkRoom = false;
        }

        private void JoinRoomOccured(INetworkRoomBs data)
        {
            IsInTheNetworkRoom = true; 
        }

        private void ConnectionLost()
        {
            IsNetworkConnected = false; 
        }

        private void ConnectionOccured()
        {
            IsNetworkConnected = true; 
        }

        private void ReceivedNetworkMessage(IMessage message)
        {

        }

        private void Unsubscribe()
        {
            INetworkApiEmitedEvents.OnLeaveRoomOccured -= LeaveRoomOccured;
            INetworkApiEmitedEvents.OnJoinRoomOccured -= JoinRoomOccured;
            INetworkApiEmitedEvents.OnConnectionLost -= ConnectionLost;
            INetworkApiEmitedEvents.OnConnectionOccured -= ConnectionOccured;
            INetworkApiEmitedEvents.OnReceivedNetworkMessage -= ReceivedNetworkMessage;

            IMessageEventsHandler.OnSendRequest -= SendRequest;
            IMessageEventsHandler.OnSendAnswer -= SendAnswer;
            IMessageEventsHandler.OnSendData -= SendData;

            IMessageEventsHandler.OnReceivedRequest -= ReceivedRequest;
            IMessageEventsHandler.OnReceivedAnswer -= ReceivedAnswer;
            IMessageEventsHandler.OnReceivedData -= ReceivedData;

            IMessageEventsHandler.OnRegisterWaitingRequest -= RegisterWaitingRequest;
            IMessageEventsHandler.OnRegisterWaitingAnswer -= RegisterWaitingAnswer;
            IMessageEventsHandler.OnRegisterWaitingData -= RegisterWaitingData;

            IMessageEventsHandler.OnRemoveWaitingRequest -= RemoveWaitingRequest;
            IMessageEventsHandler.OnRemoveWaitingAnswer -= RemoveWaitingAnswer;
            IMessageEventsHandler.OnRemoveWaitingData -= RemoveWaitingData;
        }

        private void SendRequest(IMessage message)
        {

        }

        private void SendAnswer(IMessage message)
        {

        }

        private void SendData(IMessage message)
        {

        }

        private void ReceivedRequest(IMessage message)
        {

        }

        private void ReceivedAnswer(IMessage message)
        {

        }

        private void ReceivedData(IMessage message)
        {

        }

        private void RegisterWaitingRequest(int messageId, Action<IMessage> messageProcessorAction, bool removeAfterProcessingComplete)
        {

        }

        private void RegisterWaitingAnswer(int messageId, Action<IMessage> messageProcessorAction, bool removeAfterProcessingComplete)
        {

        }

        private void RegisterWaitingData(int messageId, Action<IMessage> messageProcessorAction, bool removeAfterProcessingComplete)
        {

        }

        private void RemoveWaitingRequest(int messageId, Action<IMessage> messageProcessorAction)
        {

        }

        private void RemoveWaitingAnswer(int messageId, Action<IMessage> messageProcessorAction)
        {

        }

        private void RemoveWaitingData(int messageId, Action<IMessage> messageProcessorAction)
        {

        }
    }
}
