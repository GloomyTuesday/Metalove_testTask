namespace Scripts.BaseSystems.FileIOAndBinary
{
    public interface ISerializationTools 
    {
        public byte[] Serialize(object objToSerialize);
        public T Deserialize<T>(byte[] contentToDeserialize);
    }
}
