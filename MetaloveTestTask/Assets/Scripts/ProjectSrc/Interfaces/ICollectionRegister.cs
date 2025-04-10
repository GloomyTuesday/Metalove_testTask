using System;
using System.Collections.Generic;

namespace Scripts.ProjectSrc
{
    public interface ICollectionRegister<T>
    {
        public event Action<int, T> OnItemRegistered;
        public event Action<int> OnItemUnregistered;

        public Dictionary<int,T> InstanceIdItemIndexDictionary { get; }

        public void Register(T itemToRegister);
        public void Unregister(T itemToUnregister);
        public void Unregister(int instanceId);

        public T GetItemByInstanceId(int instanceId);

        public T[] GetRegistereItems();
    }
}
