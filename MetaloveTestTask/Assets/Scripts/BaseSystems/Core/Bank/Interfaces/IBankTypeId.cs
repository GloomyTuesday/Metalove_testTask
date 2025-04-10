using System;

namespace Scripts.BaseSystems
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"> Id used by stockpiled type.</typeparam>
    /// <typeparam name="T2"> Type that is stockpiled.</typeparam>
    public interface IBankTypeId<T,T2>
    {
        public T2[] GetItemArray();
        public T2 GetItem(T index);

        /// <summary>
        /// The parameter is an removed object id.
        /// </summary>
        public event Action<T> OnItemRemoved;

        /// <summary>
        ///   The parameter is an id of the added object
        /// </summary>
        public event Action<T> OnItemAdded;

        public void AddItem(T2 newItem, T itemId);
        public void RemoveItem(T itemId);
    }
}
