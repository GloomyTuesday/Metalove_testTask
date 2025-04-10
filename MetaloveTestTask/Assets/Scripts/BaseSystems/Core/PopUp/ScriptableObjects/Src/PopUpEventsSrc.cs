using UnityEngine;
using System; 

namespace Scripts.BaseSystems.Core
{
    [CreateAssetMenu(fileName = "PopUpEvent", menuName = "Scriptable Obj/Base systems/Core/Pop up/Pop up events")]
    public class PopUpEventsSrc : ScriptableObject, IPopUpEvents, IPopUpEventsHandler
    {
        private Action<GameObject> _onOpenPopUpByPrefab;
        event Action<GameObject> IPopUpEventsHandler.OnOpenPopUpByPrefab
        {
            add => _onOpenPopUpByPrefab = value;
            remove => _onOpenPopUpByPrefab = value; 
        }
        public void OpenPopUp(GameObject popUpPrefab) => _onOpenPopUpByPrefab?.Invoke(popUpPrefab);

        private Action _onClosePopUp;
        event Action IPopUpEventsHandler.OnClosePopUp
        {
            add => _onClosePopUp = value;
            remove => _onClosePopUp = value;
        }
        public void ClosePopUp() => _onClosePopUp?.Invoke();

        private Action _onBgCloseCanceled;
        event Action IPopUpEventsHandler.OnBgCloseCanceled
        {
            add => _onBgCloseCanceled = value;
            remove => _onBgCloseCanceled = value; 
        }
        public void BgCloseCanceled() => _onBgCloseCanceled?.Invoke();

        public Action _onBgPrepareToClose;
        event Action IPopUpEventsHandler.OnBgPrepareToClose
        {
            add => _onBgPrepareToClose = value;
            remove => _onBgPrepareToClose = value;
        }
        public void BgPrepareToClose() => _onBgPrepareToClose?.Invoke();
    }
}
