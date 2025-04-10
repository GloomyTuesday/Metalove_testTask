using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Scripts.BaseSystems.MessageProcessor
{
    [Serializable]
    public struct Message : IMessage
    {
        private byte _typeIdValue;
        private MessageTypeId _typeId;
        private int _senderGameSessionNetworkId;
        private int[] _recipientsGameSessionNetworkId;
        private InternalContentPackage _internalContentPackage;

        public byte TypeIdValue => _typeIdValue;
        public MessageTypeId TypeId => _typeId;
        public int SenderGameSessionNetworkId => _senderGameSessionNetworkId;
        public int[] RecipientsGameSessionNetworkId => _recipientsGameSessionNetworkId;
        public int MessageId => _internalContentPackage._id;
        public object MessageContent => _internalContentPackage._content;

        [Serializable]
        private struct InternalContentPackage
        {
            public int _id;
            public object _content;

            public InternalContentPackage(int id, object content)
            {
                _id = id;
                _content = content;
            }
        }

        //  ----------------------------------------    Constructors for message to send
        #region Constructors for message to send
        /// <summary>
        ///     Message to send
        /// </summary>
        public Message(
            MessageTypeId typeId,
            int senderGameSessionNetworkId,
            int[] recipientsGameSessionNetworkId,
            int messageId,
            object content
            )
        {
            _typeIdValue = (byte)typeId;
            _typeId = typeId;
            _senderGameSessionNetworkId = senderGameSessionNetworkId;
            _recipientsGameSessionNetworkId = recipientsGameSessionNetworkId;
            _internalContentPackage = new InternalContentPackage(messageId, content);
        }

        /// <summary>
        ///     Message to send
        /// </summary>
        public Message(
            byte typeIdValue,
            int senderGameSessionNetworkId,
            int messageId,
            object content
            )
        {
            _typeIdValue = typeIdValue;
            _typeId = (MessageTypeId)_typeIdValue;
            _senderGameSessionNetworkId = senderGameSessionNetworkId;
            _recipientsGameSessionNetworkId = null;
            _internalContentPackage = new InternalContentPackage(messageId, content);
        }
        #endregion

        //  ----------------------------------------    Constructors for message that is received
        #region Constructors for message that is received

        /// <summary>
        ///     Message received
        /// </summary>
        public Message(
            byte typeIdValue,
            int senderGameSessionNetworkId,
            byte[] content
            )
        {
            _typeIdValue = typeIdValue;
            _typeId = (MessageTypeId)_typeIdValue;
            _senderGameSessionNetworkId = senderGameSessionNetworkId;
            _recipientsGameSessionNetworkId = null;
            _internalContentPackage = new InternalContentPackage();
            _internalContentPackage = DeserializeContetn(content);
        }

        /// <summary>
        ///     Message received
        /// </summary>
        public Message(
            MessageTypeId typeId,
            int senderGameSessionNetworkId,
            byte[] content
            )
        {
            _typeIdValue = (byte)typeId;
            _typeId = typeId;
            _senderGameSessionNetworkId = senderGameSessionNetworkId;
            _recipientsGameSessionNetworkId = null;
            _internalContentPackage = new InternalContentPackage();
            _internalContentPackage = DeserializeContetn(content);
        }

        #endregion

        public byte[] GetMessageContentByte() => SerizlizeContent(_internalContentPackage);

        private byte[] SerizlizeContent(InternalContentPackage internalContentPackage)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (MemoryStream stream = new MemoryStream())
            {
                formatter.Serialize(stream, internalContentPackage);
                return stream.ToArray();
            }
        }

        private InternalContentPackage DeserializeContetn(byte[] contentByte)
        {
            if (contentByte == null)
                return new InternalContentPackage();

            BinaryFormatter formatter = new BinaryFormatter();

            try
            {
                using (MemoryStream stream = new MemoryStream(contentByte))
                    return (InternalContentPackage)formatter.Deserialize(stream);
                
            }
            catch (SerializationException e)
            {
                UnityEngine.Debug.Log("\t Could not deserialize byte[] in to the InternalContentPackage \t "+e.Message); 
            }

            return new InternalContentPackage();
        }

       

    }
}
