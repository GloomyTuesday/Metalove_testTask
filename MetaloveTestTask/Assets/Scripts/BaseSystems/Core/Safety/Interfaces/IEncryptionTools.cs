namespace Scripts
{
    public interface IEncryptionTools 
    {
        public byte[] EncryptSha(string input);
        public string EncryptShaAsString(string input, byte groupSize = 4);
    }
}
