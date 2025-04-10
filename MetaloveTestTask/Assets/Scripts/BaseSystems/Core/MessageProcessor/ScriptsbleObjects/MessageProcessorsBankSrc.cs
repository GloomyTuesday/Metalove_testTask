using Scripts.BaseSystems.FileIOAndBinary;
using UnityEngine;

namespace Scripts.BaseSystems.MessageProcessor
{
    [CreateAssetMenu(menuName = "Scriptable Obj/Base systems/Core/Message processor/Message processors bank")]
    public class MessageProcessorsBankSrc : ScriptableObject
    {
        [SerializeField]
        public Object[] _directoryWithProcessors;

        [SerializeField,Space(10)]
        private Object[] _messageProcessorsArray;

        [SerializeField, FilterByType(typeof(IDirectoryTools)),Space(10)]
        private Object _directoryToolsObj;

        private IDirectoryTools _iDirectoryTools;
        private IDirectoryTools IDirectoryTools
        {
            get
            {
                if (_iDirectoryTools == null)
                    _iDirectoryTools = _directoryToolsObj.GetComponent<IDirectoryTools>();

                return _iDirectoryTools;
            }
        }

#if UNITY_EDITOR
        private void OnEnable()
        {
            
        }

        private void UpdateCollections()
        {

        }
#endif  


    }
}
