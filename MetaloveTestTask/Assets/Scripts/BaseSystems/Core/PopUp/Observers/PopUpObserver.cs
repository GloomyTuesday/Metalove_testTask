using System.Collections.Generic;
using UnityEngine;

namespace Scripts.BaseSystems.Core
{
    public class PopUpObserver : MonoBehaviour, IReady
    {

        [SerializeField,Header("Object that will hold all the popups")]
        [Header("Good practice is to use game object from canvas that id different from main Ui canvas")]
        [Uneditable]
        private Transform _popUpContentHolder;
        private Transform PopUpContentHolder
        {
            get => _popUpContentHolder;
            set
            {
                _popUpContentHolder = value;
                UpdateIntegrityStatus();
            }
        }

        [SerializeField]
        [Header("This should be an object from main Ui canvas that is going to be covered with popup background")]
        [Uneditable]
        private Transform _popUpBackgroundHolder;
        private Transform PopUpBackgroundHolder
        {
            get => _popUpBackgroundHolder;
            set
            {
                _popUpBackgroundHolder = value;
                UpdateIntegrityStatus();
            }
        }

        [Space(15)]
        [SerializeField]
        [Uneditable]
        private IActiveStateAccessibleUnit[] _activeStateAccesibleCollection;

        [Space(15)]
        [Header("Pop up anim clips:")]
        [SerializeField]
        [Uneditable]
        private AnimationClip _popUpOpenAnimClip;
        private AnimationClip PopUpOpenAnimClip
        {
            get => _popUpOpenAnimClip;
            set => _popUpOpenAnimClip = value;
        }

        [Uneditable]
        [SerializeField]
        private AnimationClip _popUpCloseAnimClip;
        private AnimationClip PopUpCloseAnimClip
        {
            get => _popUpCloseAnimClip;
            set => _popUpCloseAnimClip = value;
        }

        [Space(15)]
        [Header("Back ground related")]
        [SerializeField]
        [Uneditable]
        private GameObject _popUpBgPrefab;
        private GameObject PopUpBgPrefab
        {
            get => _popUpBgPrefab;
            set => _popUpBgPrefab = value;
        }

        [SerializeField]
        [Uneditable]
        private AnimationClip _popUpAnimClipBgOpen;
        private AnimationClip PopUpAnimClipBgOpen
        {
            get => _popUpAnimClipBgOpen;
            set => _popUpAnimClipBgOpen = value;
        }

        [SerializeField]
        [Uneditable]
        private AnimationClip _popUpAnimClipBgPointerDown;
        private AnimationClip PopUpAnimClipBgPointerDown
        {
            get => _popUpAnimClipBgPointerDown;
            set => _popUpAnimClipBgPointerDown = value;
        }

        [SerializeField]
        [Uneditable]
        private AnimationClip _popUpAnimClipBgClose;
        private AnimationClip PopUpAnimClipBgClose
        {
            get => _popUpAnimClipBgClose;
            set => _popUpAnimClipBgClose = value;
        }

        [SerializeField]
        [Uneditable]
        private AnimationClip _popUpAnimClipBgCloseCanceled;
        private AnimationClip PopUpAnimClipBgCloseCanceled
        {
            get => _popUpAnimClipBgCloseCanceled;
            set => _popUpAnimClipBgCloseCanceled = value; 
        }

        [Space(15)]
        [SerializeField]
        [FilterByType(typeof(IPopUpContentHolderBuff))]
        private UnityEngine.Object _popUpContentHolderBuffObj;
        
        [SerializeField]
        [FilterByType(typeof(IPopUpEventsHandler))]
        private Object _popUpEventsObj;
        [SerializeField, FilterByType(typeof(IPopUpAnimationEventsHandler))]
        private Object _popUpAnimationEventsHandlerObj;

        public bool Ready { get; private set; }

        private Animator PopUpAnimator { get; set; }
        private Animator BgAnimator { get; set; }
        private GameObject PopUpInQueue { get; set; }
        private CurrentPopUpStateId CurrentPopUpStateId { get; set; }
        private PopUpBackgroundStateId BackgroundStateId { get; set; }
        private bool CurrentPopUpProcessing { get; set; }
        private GameObject CurrentPopUpGameObject { get; set; }

        private GameObject PopUpBackgroundObject;
        private bool ClosingFinalPopUpInProgress { get; set; } //   Should be ignore any action with popUps

        private List<IActiveStateAccessible> IActiveStateAccessibleList { get; set; } = new List<IActiveStateAccessible>(); 

        private IPopUpEventsHandler _iPopUpEventsHandler;
        private IPopUpEventsHandler IPopUpEventsHandler
        {
            get
            {
                if (_iPopUpEventsHandler == null)
                    _iPopUpEventsHandler = _popUpEventsObj.GetComponent<IPopUpEventsHandler>();

                return _iPopUpEventsHandler; 
            }
        }

        private IPopUpAnimationEventsHandler _iPopUpAnimationEventsHandler;
        private IPopUpAnimationEventsHandler IPopUpAnimationEventsHandler
        {
            get
            {
                if (_iPopUpAnimationEventsHandler == null)
                    _iPopUpAnimationEventsHandler = _popUpAnimationEventsHandlerObj.GetComponent<IPopUpAnimationEventsHandler>();

                return _iPopUpAnimationEventsHandler;
            }
        }

        private IPopUpContentHolderBuff _iPopUpContentHolderBuff;
        private IPopUpContentHolderBuff IPopUpContentHolderBuff
        {
            get
            {
                if (_iPopUpContentHolderBuff == null)
                    _iPopUpContentHolderBuff = _popUpContentHolderBuffObj.GetComponent<IPopUpContentHolderBuff>();

                return _iPopUpContentHolderBuff;
            }
        }

        [System.Serializable]
        private struct IActiveStateAccessibleUnit
        {
            [HideInInspector]
            public string _name;

            public Object _obj;

            private IActiveStateAccessible _iActiveStateAccessible;
            public IActiveStateAccessible IActiveStateAccessible
            {
                get
                {
                    if(_iActiveStateAccessible == null)
                        _iActiveStateAccessible = _obj.GetComponent<IActiveStateAccessible>();

                    return _iActiveStateAccessible;
                }
            }
        }

        private void OnValidate()
        {
            for (int i = 0; i < _activeStateAccesibleCollection.Length; i++)
                _activeStateAccesibleCollection[i]._name = _activeStateAccesibleCollection[i]._obj.name; 
        }

        private void OnEnable()
        {
            PopUpContentHolder = IPopUpContentHolderBuff.PopUpContentHolder;
            PopUpBackgroundHolder = IPopUpContentHolderBuff.PopUpBackgroundHolder;

            CreatePopUpBackgroundObject();
            Subscribe();
            Ready = true; 
        }

        private void OnDisable()
        {
            Unsubscribe();

            if (PopUpBackgroundObject != null )
                Destroy(PopUpBackgroundObject); 
        }

        private void Subscribe()
        {
            IPopUpContentHolderBuff.OnPopUpBgPrefabUpdated += PopUpBgPrefabUpdated;

            IPopUpContentHolderBuff.OnPopUpContentHolderUpdated += PopUpContentHolderUpdated;
            IPopUpContentHolderBuff.OnPopUpBackgroundHolderUpdated += PopUpBackgroundHolderUpdated;

            IPopUpContentHolderBuff.OnPopUpAnimClipOpenUpdated += PopUpAnimClipOpenUpdated;
            IPopUpContentHolderBuff.OnPopUpAnimClipCloseUpdated += PopUpAnimClipCloseUpdated;

            IPopUpContentHolderBuff.OnPopUpAnimClipBgPointerDownUpdated += PopUpAnimClipBgPointerDownUpdated;
            IPopUpContentHolderBuff.OnPopUpAnimClipBgOpenUpdated += PopUpAnimClipBgOpenUpdated;
            IPopUpContentHolderBuff.OnPopUpAnimClipBgCloseUpdated += PopUpAnimClipBgCloseUpdated;
            IPopUpContentHolderBuff.OnPopUpAnimClipBgCloseCanceledUpdated += PopUpAnimClipBgCloseCanceledUpdated;

            IPopUpEventsHandler.OnOpenPopUpByPrefab += OpenPopUpByPrefab;
            IPopUpEventsHandler.OnClosePopUp += ClosePopUp;
            IPopUpEventsHandler.OnBgPrepareToClose += BgPrepareToClose;
            IPopUpEventsHandler.OnBgCloseCanceled += BgCloseCanceled;

            IPopUpAnimationEventsHandler.OnPopUpAnimOpenStart += PopUpAnimOpenStart;
            IPopUpAnimationEventsHandler.OnPopUpAnimOpenEnd += PopUpAnimOpenEnd;
            IPopUpAnimationEventsHandler.OnPopUpAnimCloseStart += PopUpAnimCloseStart;
            IPopUpAnimationEventsHandler.OnPopUpAnimCloseEnd += PopUpAnimCloseEnd;
            IPopUpAnimationEventsHandler.OnBgAnimOpenStart += BgAnimOpenStart;
            IPopUpAnimationEventsHandler.OnBgAnimOpenEnd += BgAnimOpenEnd;
            IPopUpAnimationEventsHandler.OnBgAnimPointerDownStart += BgAnimPointerDownStart;
            IPopUpAnimationEventsHandler.OnBgAnimPointerDownEnd += BgAnimPointerDownEnd;
            IPopUpAnimationEventsHandler.OnBgAnimCloseCancelStart += BgAnimCloseCancelStart;
            IPopUpAnimationEventsHandler.OnBgAnimCloseCancelEnd += BgAnimCloseCancelEnd;
            IPopUpAnimationEventsHandler.OnBgAnimCloseStart += BgAnimCloseStart;
            IPopUpAnimationEventsHandler.OnBgAnimCloseEnd += BgAnimCloseEnd;
        }

        private void Unsubscribe()
        {
            IPopUpContentHolderBuff.OnPopUpBgPrefabUpdated -= PopUpBgPrefabUpdated;

            IPopUpContentHolderBuff.OnPopUpContentHolderUpdated -= PopUpContentHolderUpdated;
            IPopUpContentHolderBuff.OnPopUpBackgroundHolderUpdated -= PopUpBackgroundHolderUpdated;

            IPopUpContentHolderBuff.OnPopUpAnimClipOpenUpdated -= PopUpAnimClipOpenUpdated;
            IPopUpContentHolderBuff.OnPopUpAnimClipCloseUpdated -= PopUpAnimClipCloseUpdated;

            IPopUpContentHolderBuff.OnPopUpAnimClipBgPointerDownUpdated -= PopUpAnimClipBgPointerDownUpdated;
            IPopUpContentHolderBuff.OnPopUpAnimClipBgOpenUpdated -= PopUpAnimClipBgOpenUpdated;
            IPopUpContentHolderBuff.OnPopUpAnimClipBgCloseUpdated -= PopUpAnimClipBgCloseUpdated;
            IPopUpContentHolderBuff.OnPopUpAnimClipBgCloseCanceledUpdated -= PopUpAnimClipBgCloseCanceledUpdated;

            IPopUpEventsHandler.OnOpenPopUpByPrefab -= OpenPopUpByPrefab;
            IPopUpEventsHandler.OnClosePopUp -= ClosePopUp;
            IPopUpEventsHandler.OnBgPrepareToClose -= BgPrepareToClose;
            IPopUpEventsHandler.OnBgCloseCanceled -= BgCloseCanceled;

            IPopUpAnimationEventsHandler.OnPopUpAnimOpenStart -= PopUpAnimOpenStart;
            IPopUpAnimationEventsHandler.OnPopUpAnimOpenEnd -= PopUpAnimOpenEnd;
            IPopUpAnimationEventsHandler.OnPopUpAnimCloseStart -= PopUpAnimCloseStart;
            IPopUpAnimationEventsHandler.OnPopUpAnimCloseEnd -= PopUpAnimCloseEnd;
            IPopUpAnimationEventsHandler.OnBgAnimOpenStart -= BgAnimOpenStart;
            IPopUpAnimationEventsHandler.OnBgAnimOpenEnd -= BgAnimOpenEnd;
            IPopUpAnimationEventsHandler.OnBgAnimPointerDownStart -= BgAnimPointerDownStart;
            IPopUpAnimationEventsHandler.OnBgAnimPointerDownEnd -= BgAnimPointerDownEnd;
            IPopUpAnimationEventsHandler.OnBgAnimCloseCancelStart -= BgAnimCloseCancelStart;
            IPopUpAnimationEventsHandler.OnBgAnimCloseCancelEnd -= BgAnimCloseCancelEnd;
            IPopUpAnimationEventsHandler.OnBgAnimCloseStart -= BgAnimCloseStart;
            IPopUpAnimationEventsHandler.OnBgAnimCloseEnd -= BgAnimCloseEnd;
        }

        private void PopUpBgPrefabUpdated(GameObject popUpBgPrefab) => PopUpBgPrefab = popUpBgPrefab;
        
        private void PopUpAnimClipCloseUpdated(AnimationClip popUpAnimClipBgCloseCanceled) => PopUpAnimClipBgCloseCanceled = popUpAnimClipBgCloseCanceled;
        private void PopUpAnimClipOpenUpdated(AnimationClip popUpAnimClipBgCloseCanceled) => PopUpAnimClipBgCloseCanceled = popUpAnimClipBgCloseCanceled;
        private void PopUpBgHolderUpdated(AnimationClip popUpAnimClipBgCloseCanceled) => PopUpAnimClipBgCloseCanceled = popUpAnimClipBgCloseCanceled;
        private void PopUpAnimClipBgCloseCanceledUpdated(AnimationClip popUpAnimClipBgCloseCanceled) => PopUpAnimClipBgCloseCanceled = popUpAnimClipBgCloseCanceled;
        private void PopUpAnimClipBgCloseUpdated(AnimationClip popUpAnimClipBgClose) => PopUpAnimClipBgClose = popUpAnimClipBgClose;
        private void PopUpAnimClipBgOpenUpdated(AnimationClip popUpAnimClipBgOpen) => PopUpAnimClipBgOpen = popUpAnimClipBgOpen;
        private void PopUpAnimClipBgPointerDownUpdated(AnimationClip popUpAnimClipBgPointerDown) => PopUpAnimClipBgPointerDown = popUpAnimClipBgPointerDown;

        private void PopUpContentHolderUpdated(Transform popUpContentHolder) => PopUpContentHolder = popUpContentHolder;
        private void PopUpBackgroundHolderUpdated(Transform popUpBackgroundHolder) => PopUpBackgroundHolder = popUpBackgroundHolder;

        private void UpdateIntegrityStatus()
        {
            if (_popUpContentHolder == null)
            {
                Ready = false;
                return;
            }

            Ready = true;
        }
        private void CreatePopUpBackgroundObject()
        {
            if (_popUpBgPrefab == null) return;

            PopUpBackgroundObject = Instantiate(_popUpBgPrefab , _popUpBackgroundHolder.transform);
            BgAnimator = PopUpBackgroundObject.GetComponent<Animator>();
           
            PopUpBackgroundObject.SetActive(false);
        }

        private void OpenPopUpByPrefab(GameObject popUpPrefab)
        {
            ActiveStateAccesibleCollectionValue(false);

            if (ClosingFinalPopUpInProgress) return;

            if (
                CurrentPopUpStateId == CurrentPopUpStateId.Opening ||
                CurrentPopUpStateId == CurrentPopUpStateId.Closing
                )
            {
                PopUpInQueue = popUpPrefab;
                return;
            }

            if (CurrentPopUpStateId == CurrentPopUpStateId.Non)
            {
                InstantiateNewPopUp(popUpPrefab);

                PopUpBackgroundObject?.SetActive(true);

                BgAnimator.Play(_popUpAnimClipBgOpen.name);
            }
        }
        
        private void ClosePopUp()
        {
            ActiveStateAccesibleCollectionValue(true);
            if (ClosingFinalPopUpInProgress) return;

            if (PopUpInQueue == null)
                BgAnimator.Play(_popUpAnimClipBgClose.name); 

            CurrentPopUpStateId = CurrentPopUpStateId.Closing;
            PopUpAnimator.Play(_popUpCloseAnimClip.name);
        }

        private void BgPrepareToClose()
        {
            if (ClosingFinalPopUpInProgress) return;

            BgAnimator.Play(_popUpAnimClipBgPointerDown.name); 
        }

        private void BgCloseCanceled()
        {
            if (ClosingFinalPopUpInProgress) return;

            BgAnimator.Play(_popUpAnimClipBgCloseCanceled.name); 
        }

        private void InstantiateNewPopUp(GameObject popUpPrefab)
        {
            CurrentPopUpStateId = CurrentPopUpStateId.Opening;

            CurrentPopUpGameObject = Instantiate(popUpPrefab,_popUpContentHolder);
            CurrentPopUpGameObject.transform.localPosition = Vector3.zero;
            CurrentPopUpGameObject.transform.localScale = Vector3.one;

            PopUpAnimator = CurrentPopUpGameObject.GetComponent<Animator>();

            PopUpAnimator.Play(_popUpOpenAnimClip.name); 
        }

        private void ActiveStateAccesibleCollectionValue(bool state)
        {
            foreach (var item in _activeStateAccesibleCollection)
                item.IActiveStateAccessible.ActiveState = state;
        }

        #region PopUp animation events methods

        private void PopUpAnimOpenStart()
        {
            CurrentPopUpStateId = CurrentPopUpStateId.Active;
        }

        private void PopUpAnimOpenEnd()
        {
            if (PopUpInQueue == null) return;

            ClosePopUp(); 
        }

        private void PopUpAnimCloseStart()
        {
            CurrentPopUpStateId = CurrentPopUpStateId.Closing;

            if (PopUpInQueue==null)
                BgAnimator.Play(_popUpAnimClipBgClose.name); 

        }

        private void PopUpAnimCloseEnd()
        {
            Destroy(CurrentPopUpGameObject);

            if (PopUpInQueue!=null)
            {
                InstantiateNewPopUp(PopUpInQueue);
                PopUpInQueue = null;
            }
        }

        private void BgAnimOpenStart()
        {
            PopUpBackgroundObject?.SetActive(true);
        }

        private void BgAnimOpenEnd()
        {

        }

        private void BgAnimPointerDownStart()
        {

        }

        private void BgAnimPointerDownEnd()
        {

        }

        private void BgAnimCloseCancelStart()
        {

        }

        private void BgAnimCloseCancelEnd()
        {

        }

        private void BgAnimCloseStart()
        {
            ClosingFinalPopUpInProgress = true; 
        }

        private void BgAnimCloseEnd()
        {
            ClosingFinalPopUpInProgress = false; 
            CurrentPopUpStateId = CurrentPopUpStateId.Non;
            PopUpBackgroundObject?.SetActive(false);
        }

        #endregion
    }
}
