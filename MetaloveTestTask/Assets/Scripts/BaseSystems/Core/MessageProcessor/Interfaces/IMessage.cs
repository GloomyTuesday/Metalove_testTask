namespace Scripts.BaseSystems.MessageProcessor
{
    public interface IMessage 
    {
        /// <summary>
        /// Message type id is answering the question: Is it a request, answer or data
        /// </summary>
        public byte TypeIdValue { get; }
        public MessageTypeId TypeId { get; }
        public int SenderGameSessionNetworkId { get; }

        /// <summary>
        ///     Null value would mean that message will be sent to everyone
        /// </summary>
        public int[] RecipientsGameSessionNetworkId { get; }
        public int MessageId { get; }
        public object MessageContent { get; }
        public byte[] GetMessageContentByte();
    }
}
