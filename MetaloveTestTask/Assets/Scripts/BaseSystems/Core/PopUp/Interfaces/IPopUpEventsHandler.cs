using System;
using UnityEngine;

namespace Scripts.BaseSystems.Core
{
    internal interface IPopUpEventsHandler
    {
        public event Action<GameObject> OnOpenPopUpByPrefab;
        public event Action OnClosePopUp;
        public event Action OnBgCloseCanceled;
        public event Action OnBgPrepareToClose;
    }
}
