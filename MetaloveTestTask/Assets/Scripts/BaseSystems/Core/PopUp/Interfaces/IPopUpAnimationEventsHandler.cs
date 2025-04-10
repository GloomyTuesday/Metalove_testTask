using System;

namespace Scripts.BaseSystems.Core
{
    internal interface IPopUpAnimationEventsHandler
    {
        public event Action OnPopUpAnimOpenStart;
        public event Action OnPopUpAnimOpenEnd;
        public event Action OnPopUpAnimCloseStart;
        public event Action OnPopUpAnimCloseEnd;
        public event Action OnBgAnimOpenStart;
        public event Action OnBgAnimOpenEnd;
        public event Action OnBgAnimPointerDownStart;
        public event Action OnBgAnimPointerDownEnd;
        public event Action OnBgAnimCloseCancelStart;
        public event Action OnBgAnimCloseCancelEnd;
        public event Action OnBgAnimCloseStart;
        public event Action OnBgAnimCloseEnd;
    }
}
