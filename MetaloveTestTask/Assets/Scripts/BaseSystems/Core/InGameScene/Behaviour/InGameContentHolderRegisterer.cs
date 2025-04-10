using UnityEngine;

namespace Scripts.BaseSystems.Core
{
    public class InGameContentHolderRegisterer : MonoBehaviour
    {
        [SerializeField]
        private InGameSceneContentHolderId _contentType; 

        [Space(15)]
        [SerializeField, FilterByType(typeof(IInGameSceneContentHolderBuffer))]
        private Object _inGameSceneContentHolderBufferObj;

        private IInGameSceneContentHolderBuffer _iInGameSceneContentHolderBuffer;
        private IInGameSceneContentHolderBuffer IInGameSceneContentHolderBuffer
        {
            get
            {
                if (_iInGameSceneContentHolderBuffer == null)
                    _iInGameSceneContentHolderBuffer = _inGameSceneContentHolderBufferObj.GetComponent<IInGameSceneContentHolderBuffer>();

                return _iInGameSceneContentHolderBuffer;
            }
        }

        private int InstanceId { get; set; }

        private void OnEnable()
        {
            InstanceId = GetInstanceID(); 

            switch (_contentType)
            {
                case InGameSceneContentHolderId.CanvasContentHolder:
                    IInGameSceneContentHolderBuffer.CanvasContentHolder = transform; 
                    break;

                case InGameSceneContentHolderId.WorldContentHolder:
                    IInGameSceneContentHolderBuffer.WorldContentHolder = transform;
                    break; 

                case InGameSceneContentHolderId.RootContentHolder:
                    IInGameSceneContentHolderBuffer.RootContentHolder = transform;
                    break;

                case InGameSceneContentHolderId.InGameSceneInstanceHolder:
                    IInGameSceneContentHolderBuffer.InstanceHolderTransform = transform;
                    break;
            }
        }

        private void OnDisable()
        {
            IInGameSceneContentHolderBuffer.Unregister(InstanceId); 
        }

    }
}
