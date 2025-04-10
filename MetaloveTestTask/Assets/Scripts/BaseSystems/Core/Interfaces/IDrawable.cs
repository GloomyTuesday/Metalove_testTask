namespace Scripts.BaseSystems.Interfaces
{
    public interface IDrawable 
    {
        public bool DrawFlag { get; }
        public void Draw(bool drawFlag = true); 
    }
}

