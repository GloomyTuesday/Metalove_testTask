using System;
using UnityEngine;

namespace Scripts.BaseSystems.Core
{
    public interface IInGameSceneContentHolderBuffer
    {
        public Transform CanvasContentHolder {get;set;}
        public event Action<Transform> OnCanvasContentHolderUpdated;

        public Transform WorldContentHolder { get; set; }
        public event Action<Transform> OnWorldContentHolderUpdated;

        public Transform RootContentHolder { get; set; }
        public event Action<Transform> OnRootContentHolderUpdated;

        public Transform InstanceHolderTransform { get; set; }
        public event Action<Transform> OnInstanceHolderUpdated;

        public void Unregister(Transform contentHolder);
        public void Unregister(int contentHolderInstanceId);
    }
}
