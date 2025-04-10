namespace Scripts
{
    /*
        Is converted in to the byte value when is used as event Id for creting network package
        For some network Api value 0 may not work.
     */ 
    public enum MessageTypeId
    {
        Request = 1,
        Answer = 2,
        Data = 3
    }
}
