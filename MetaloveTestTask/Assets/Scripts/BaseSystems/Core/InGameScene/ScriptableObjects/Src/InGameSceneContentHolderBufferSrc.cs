using System;
using UnityEngine;

namespace Scripts.BaseSystems.Core
{
    [CreateAssetMenu(fileName = "InGameSceneContentHolderBuffer", menuName = "Scriptable Obj/Base systems/Core/In game scene/In game scene content holder buffer")]
    public class InGameSingleSceneBankSrc : ScriptableObject, IInGameSceneContentHolderBuffer
    {
        [Header("Registered holder objects names:")]
        [SerializeField]
        [Uneditable]
        private string _canvasContentHolder;

        private int _canvasContentHolderTransformInstanceId; 
        private Transform _canvasContentHolderTransform;
        public Transform CanvasContentHolder 
        {
            get => _canvasContentHolderTransform; 
            set
            {
                _canvasContentHolderTransform = value;
                _canvasContentHolderTransformInstanceId = _canvasContentHolderTransform.GetInstanceID();
                _canvasContentHolder = _canvasContentHolderTransform != null ? _canvasContentHolderTransform.name : "";
                OnCanvasContentHolderUpdated?.Invoke(_canvasContentHolderTransform);
            }
        }
        public event Action<Transform> OnCanvasContentHolderUpdated;

        [SerializeField]
        [Uneditable]
        private string _worldContentHolder;

        private int _worldContentHolderTransformInstanceId;
        private Transform _worldContentHolderTransforTransform;
        public Transform WorldContentHolder
        {
            get => _worldContentHolderTransforTransform; 
            set
            {
                _worldContentHolderTransforTransform = value;
                _worldContentHolderTransformInstanceId = _worldContentHolderTransforTransform.GetInstanceID(); 
                _worldContentHolder = _worldContentHolderTransforTransform != null ? _worldContentHolderTransforTransform.name : "";
                OnWorldContentHolderUpdated?.Invoke(_worldContentHolderTransforTransform); 
            }
        }
        public event Action<Transform> OnWorldContentHolderUpdated;

        [SerializeField]
        [Uneditable]
        private string _rootContentHolder;

        private int _rootContentHolderTransformInstanceId;
        private Transform _rootContentHolderTransforTransform;
        public Transform RootContentHolder
        {
            get => _rootContentHolderTransforTransform; 
            set
            {
                _rootContentHolderTransforTransform = value;
                _rootContentHolderTransformInstanceId = _rootContentHolderTransforTransform.GetInstanceID(); 
                _rootContentHolder = _rootContentHolderTransforTransform != null ? _rootContentHolderTransforTransform.name : "";
                OnRootContentHolderUpdated?.Invoke(_rootContentHolderTransforTransform); 
            }
        }
        public event Action<Transform> OnRootContentHolderUpdated;

        [SerializeField]
        [Uneditable]
        private string _inGameSceneInstanceHolder;

        private int _inGameSceneInstanceHolderTransformInstanceId;
        private Transform _inGameSceneInstanceHolderTransform;
        public Transform InstanceHolderTransform
        {
            get => _inGameSceneInstanceHolderTransform;
            set
            {
                _inGameSceneInstanceHolderTransform = value;
                _inGameSceneInstanceHolderTransformInstanceId = _inGameSceneInstanceHolderTransform.GetInstanceID();
                _inGameSceneInstanceHolder = _inGameSceneInstanceHolderTransform != null ? _inGameSceneInstanceHolderTransform.name : "";
                OnInstanceHolderUpdated?.Invoke(_inGameSceneInstanceHolderTransform);
            }
        }
        public event Action<Transform> OnInstanceHolderUpdated;

        public void Unregister(Transform contentHolder)
        {
            if(CanvasContentHolder == contentHolder)
            {
                CanvasContentHolder = null; 
                return;
            }

            if (WorldContentHolder == contentHolder)
            {
                WorldContentHolder = null;
                return;
            }

            if (RootContentHolder == contentHolder)
            {
                RootContentHolder = null;
                return;
            }
        }

        public void Unregister(int contentHolderInstanceId)
        {
            if (_canvasContentHolderTransformInstanceId == contentHolderInstanceId)
            {
                CanvasContentHolder = null;
                return;
            }

            if (_worldContentHolderTransformInstanceId == contentHolderInstanceId)
            {
                WorldContentHolder = null;
                return;
            }

            if (_rootContentHolderTransformInstanceId == contentHolderInstanceId)
            {
                RootContentHolder = null;
                return;
            }
        }
    }
}

