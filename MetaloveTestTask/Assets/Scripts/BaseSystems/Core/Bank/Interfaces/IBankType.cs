using System;

namespace Scripts.BaseSystems
{
    public interface IBankType<T>
    {
        /// <summary>
        /// The parameter is an removed object Instance Id.
        /// </summary>
        public event Action<int> OnItemRemoved;

        /// <summary>
        ///   The parameter is an instance of the added object
        /// </summary>
        public event Action<int> OnItemAdded;

        public T[] GetItemArray();

        public T GetItemByInstanceId(int instanceId);

        public void AddItem(T newItem);
        public void RemoveItem(int instanceId);
        public bool ContainsItem(int instanceId);
    }
}
