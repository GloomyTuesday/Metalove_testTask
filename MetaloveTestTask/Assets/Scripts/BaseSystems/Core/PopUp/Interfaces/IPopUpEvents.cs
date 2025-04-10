using UnityEngine;

namespace Scripts.BaseSystems.Core
{
    public interface IPopUpEvents
    {
        public void OpenPopUp(GameObject popUpPrefab);
        public void ClosePopUp();
        public void BgCloseCanceled();
        public void BgPrepareToClose();
    }
}
