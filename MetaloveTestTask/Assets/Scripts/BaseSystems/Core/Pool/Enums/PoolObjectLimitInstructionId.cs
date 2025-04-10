namespace Scripts.BaseSystems.Pool
{
    public enum PoolObjectLimitInstructionId
    {
        AlwaysCreateANewObject = 0 ,        //  Limitless poll
        UseOldest = 1,       //  Use oldest object
        Use“eglected = 2     //  Use object that has oldest last time usage date
    }
}
