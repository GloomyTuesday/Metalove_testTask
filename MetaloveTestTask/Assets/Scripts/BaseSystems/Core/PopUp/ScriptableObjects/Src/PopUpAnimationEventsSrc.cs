using UnityEngine;
using System; 

namespace Scripts.BaseSystems.Core
{
    [CreateAssetMenu(fileName = "PopUpAnimationEvents", menuName = "Scriptable Obj/Base systems/Core/Pop up/Pop up animation events")]
    public class PopUpAnimationEventsSrc : ScriptableObject, IPopUpAnimationEvents, IPopUpAnimationEventsHandler
    {

        private Action _onPopUpAnimOpenStart;
        event Action IPopUpAnimationEventsHandler.OnPopUpAnimOpenStart
        {
            add => _onPopUpAnimOpenStart = value;
            remove => _onPopUpAnimOpenStart = value;
        }
        public void PopUpAnimOpenStart() => _onPopUpAnimOpenStart?.Invoke();

        private Action _onPopUpAnimOpenEnd;
        event Action IPopUpAnimationEventsHandler.OnPopUpAnimOpenEnd
        {
            add => _onPopUpAnimOpenEnd = value;
            remove => _onPopUpAnimOpenEnd = value;
        }
        public void PopUpAnimOpenEnd() => _onPopUpAnimOpenEnd?.Invoke();

        private Action _onPopUpAnimCloseStart;
        event Action IPopUpAnimationEventsHandler.OnPopUpAnimCloseStart
        {
            add => _onPopUpAnimCloseStart = value;
            remove => _onPopUpAnimCloseStart = value;
        }
        public void PopUpAnimCloseStart() => _onPopUpAnimCloseStart?.Invoke();

        private Action _onPopUpAnimCloseEnd;
        event Action IPopUpAnimationEventsHandler.OnPopUpAnimCloseEnd
        {
            add => _onPopUpAnimCloseEnd = value;
            remove => _onPopUpAnimCloseEnd = value;
        }
        public void PopUpAnimCloseEnd() => _onPopUpAnimCloseEnd?.Invoke();

        private Action _onBgAnimOpenStart;
        event Action IPopUpAnimationEventsHandler.OnBgAnimOpenStart
        {
            add => _onBgAnimOpenStart = value;
            remove => _onBgAnimOpenStart = value;
        }
        public void BgAnimOpenStart() => _onBgAnimOpenStart?.Invoke();

        private Action _onBgAnimOpenEnd;
        event Action IPopUpAnimationEventsHandler.OnBgAnimOpenEnd
        {
            add => _onBgAnimOpenEnd = value;
            remove => _onBgAnimOpenEnd = value;
        }
        public void BgAnimOpenEnd() => _onBgAnimOpenEnd?.Invoke();

        private Action _onBgAnimPointerDownStart;
        event Action IPopUpAnimationEventsHandler.OnBgAnimPointerDownStart
        {
            add => _onBgAnimPointerDownStart = value;
            remove => _onBgAnimPointerDownStart = value;
        }
        public void BgAnimPointerDownStart() => _onBgAnimPointerDownStart?.Invoke();

        private Action _onBgAnimPointerDownEnd;
        event Action IPopUpAnimationEventsHandler.OnBgAnimPointerDownEnd
        {
            add => _onBgAnimPointerDownEnd = value;
            remove => _onBgAnimPointerDownEnd = value;
        }
        public void BgAnimPointerDownEnd() => _onBgAnimPointerDownEnd?.Invoke();

        private Action _onBgAnimCloseCancelStart;
        event Action IPopUpAnimationEventsHandler.OnBgAnimCloseCancelStart
        {
            add => _onBgAnimCloseCancelStart = value;
            remove => _onBgAnimCloseCancelStart = value;
        }
        public void BgAnimCloseCancelStart() => _onBgAnimCloseCancelStart?.Invoke();

        private Action _onBgAnimCloseCancelEnd;
        event Action IPopUpAnimationEventsHandler.OnBgAnimCloseCancelEnd
        {
            add => _onBgAnimCloseCancelEnd = value;
            remove => _onBgAnimCloseCancelEnd = value;
        }
        public void BgAnimCloseCancelEnd() => _onBgAnimCloseCancelEnd?.Invoke();

        private Action _onBgAnimCloseStart;
        event Action IPopUpAnimationEventsHandler.OnBgAnimCloseStart
        {
            add => _onBgAnimCloseStart = value;
            remove => _onBgAnimCloseStart = value;
        }
        public void BgAnimCloseStart() => _onBgAnimCloseStart?.Invoke();

        private Action _onBgAnimCloseEnd;
        event Action IPopUpAnimationEventsHandler.OnBgAnimCloseEnd
        {
            add => _onBgAnimCloseEnd = value;
            remove => _onBgAnimCloseEnd = value;
        }
        public void BgAnimCloseEnd() => _onBgAnimCloseEnd?.Invoke();
    }
}
