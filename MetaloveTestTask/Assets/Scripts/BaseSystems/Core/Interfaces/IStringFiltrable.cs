namespace Scripts.BaseSystems
{
    public interface IStringFiltrable 
    {
        public void SetFilterState(bool active);
        public bool GetFilterState();
        public string FilterString { get; set; }
    }
}
