using UnityEngine;

namespace Scripts.BaseSystems.Core
{
    public class PopUpAnimEventHolderBehaviour : MonoBehaviour
    {
        [SerializeField]
        private PopUpAnimationEventsSrc _popUpAnimEvent;

        private void PopUpAnimOpenStart() => _popUpAnimEvent.PopUpAnimOpenStart();
        private void PopUpAnimOpenEnd() => _popUpAnimEvent.PopUpAnimOpenEnd();
        private void PopUpAnimCloseStart() => _popUpAnimEvent.PopUpAnimCloseStart();
        private void PopUpAnimCloseEnd() => _popUpAnimEvent.PopUpAnimCloseEnd();
        private void BgAnimOpenStart() => _popUpAnimEvent.BgAnimOpenStart();
        private void BgAnimOpenEnd() => _popUpAnimEvent.BgAnimOpenEnd();
        private void BgAnimPointerDownStart() => _popUpAnimEvent.BgAnimPointerDownStart();
        private void BgAnimPointerDownEnd() => _popUpAnimEvent.BgAnimPointerDownEnd();
        private void BgAnimCloseCancelStart() => _popUpAnimEvent.BgAnimCloseCancelStart();
        private void BgAnimCloseCancelEnd() => _popUpAnimEvent.BgAnimCloseCancelEnd();
        private void BgAnimCloseStart() => _popUpAnimEvent.BgAnimCloseStart();
        private void BgAnimCloseEnd() => _popUpAnimEvent.BgAnimCloseEnd();
    }
}
