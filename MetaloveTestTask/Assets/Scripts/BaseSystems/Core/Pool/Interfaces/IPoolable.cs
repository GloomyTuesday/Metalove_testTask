namespace Scripts.BaseSystems.Pool
{
    public interface IPoolable<T>
    {
        public void CopyDataFrom( T dataDonor );
        public void SetActive(bool activeState);
        public object ObjectMain { get; }
    }
}
