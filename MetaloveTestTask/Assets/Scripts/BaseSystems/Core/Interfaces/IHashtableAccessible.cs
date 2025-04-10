using System.Collections.Generic;

namespace Scripts.BaseSystems
{
    public interface IHashtableAccessible
    {
        public int GetHashtableCount();
        public void SetHashtableValue(object key, object value);
        public T GetHashtableValue<T>(object key);
        public IEnumerator<KeyValuePair<object, object>> GetHashtableEnumerator();
    }
}
