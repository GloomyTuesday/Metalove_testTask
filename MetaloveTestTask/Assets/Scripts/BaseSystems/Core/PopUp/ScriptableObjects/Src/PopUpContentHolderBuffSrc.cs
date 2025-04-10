using System;
using UnityEngine;

namespace Scripts.BaseSystems.Core
{
    [CreateAssetMenu(fileName = "PopUpContentHolderBuff", menuName = "Scriptable Obj/Base systems/Core/Pop up/Pop up content holder buff")]
    public class PopUpContentHolderBuffSrc : ScriptableObject, IPopUpContentHolderBuff
    {
        [Header("Registered objects names:")]
        [SerializeField]
        [Uneditable]
        private string _backgroundHolder;

        private Transform _backgroundHolderTransform;
        public Transform PopUpBackgroundHolder
        {
            get => _backgroundHolderTransform;
            set
            {
                if (_backgroundHolderTransform == value) return;
                _backgroundHolderTransform = value;
                _backgroundHolder = _backgroundHolderTransform.name;
                OnPopUpBackgroundHolderUpdated?.Invoke(_backgroundHolderTransform); 
            }
        }
        public event Action<Transform> OnPopUpBackgroundHolderUpdated;


        [SerializeField]
        [Uneditable]
        private string _contentHolder;

        private Transform _popUpContentHolderTransform;
        public Transform PopUpContentHolder
        {
            get => _popUpContentHolderTransform; 
            set
            {
                if (_popUpContentHolderTransform == value) return;
                _popUpContentHolderTransform = value;
                _contentHolder = _popUpContentHolderTransform.name;
                OnPopUpContentHolderUpdated?.Invoke(_popUpContentHolderTransform); 
            }
        }
        public event Action<Transform> OnPopUpContentHolderUpdated;

        [Space(15)]
        [Header("Pop up animations clips:")]
        [SerializeField]
        private AnimationClip _popUpOpen;
        public AnimationClip PopUpAnimClipOpen 
        { 
            get=> _popUpOpen;
            set
            {
                if (_bgOpen == value) return; 

                _bgOpen = value;
                OnPopUpAnimClipOpenUpdated?.Invoke(_bgOpen); 
            } 
        }
        public event Action<AnimationClip> OnPopUpAnimClipOpenUpdated;

        [SerializeField]
        private AnimationClip _popUpClose;
        public AnimationClip PopUpAnimClipClose 
        { 
            get => _popUpClose;
            set
            {
                if (_popUpClose == value) return;

                _popUpClose = value;
                OnPopUpAnimClipCloseUpdated?.Invoke(_popUpClose); 
            }
        }
        public event Action<AnimationClip> OnPopUpAnimClipCloseUpdated;

        [Space(15)]
        [SerializeField]
        private AnimationClip _bgOpen;
        public AnimationClip PopUpAnimClipBgOpen 
        { 
            get => _bgOpen;
            set
            {
                if (_bgOpen == value) return;

                _bgOpen = value;
                OnPopUpAnimClipBgOpenUpdated?.Invoke(_bgOpen);
            }
        }
        public event Action<AnimationClip> OnPopUpAnimClipBgOpenUpdated;

        [SerializeField]
        private AnimationClip _bgClose;
        public AnimationClip PopUpAnimClipBgClose 
        {
            get => _bgClose;
            set
            {
                if (_bgClose == value) return;

                _bgClose = value;
                OnPopUpAnimClipBgCloseUpdated?.Invoke(_bgClose);
            }
        }
        public event Action<AnimationClip> OnPopUpAnimClipBgCloseUpdated;

        [SerializeField]
        private AnimationClip _bgPointerDown;
        public AnimationClip PopUpAnimClipBgPointerDown 
        { 
            get => _bgPointerDown;
            set
            {
                if (_bgPointerDown == value) return;

                _bgPointerDown = value;
                OnPopUpAnimClipBgPointerDownUpdated?.Invoke(_bgPointerDown);
            }
        }
        public event Action<AnimationClip> OnPopUpAnimClipBgPointerDownUpdated;

        [SerializeField]
        private AnimationClip _bgCloseCanceled;
        public AnimationClip PopUpAnimClipBgCloseCanceled 
        { 
            get => _bgCloseCanceled;
            set
            {
                if (_bgCloseCanceled == value) return;

                _bgCloseCanceled = value;
                OnPopUpAnimClipBgCloseCanceledUpdated?.Invoke(_bgCloseCanceled);
            }
        }
        public event Action<AnimationClip> OnPopUpAnimClipBgCloseCanceledUpdated;

        [Space(15)]
        [SerializeField]
        private GameObject _backgroundPrefab;
        public GameObject PopUpBgPrefab 
        {  
            get => _backgroundPrefab;
            set
            {
                if (_backgroundPrefab == value) return;

                _backgroundPrefab = value;
                OnPopUpBgPrefabUpdated?.Invoke(_backgroundPrefab);
            }
        }
        public event Action<GameObject> OnPopUpBgPrefabUpdated;
    }
}

