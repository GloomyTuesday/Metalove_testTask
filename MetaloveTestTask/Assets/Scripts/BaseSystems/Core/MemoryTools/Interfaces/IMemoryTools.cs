namespace Scripts.BaseSystems
{
    public interface IMemoryTools 
    {
        public float GetUsedMemory { get; }
        public void GCCollect(); 
    }
}
