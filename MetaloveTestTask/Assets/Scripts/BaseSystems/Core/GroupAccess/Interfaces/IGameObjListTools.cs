using System;
using UnityEngine;

namespace Scripts.BaseSystems.GameObjListTools
{
    public interface IGameObjListTools 
    {
        public void TryToRegisterActivator(string key, Action activationCallback, Action deactivationCallback, GameObject obj = null);
        public void TryToUnregisterActivator(string key, Action activationCallback, Action deactivationCallback, GameObject obj = null);

        public void TryToRegisterActivalable(string key, Action activationCallback, Action deactivationCallback, GameObject obj = null);
        public void TryToUnregisterActivalable(string key, Action activationCallback, Action deactivationCallback, GameObject obj = null);

        public void Activate(string key);
        public void Deactiate(string key);

        public void DiactivateAll();
    }
}
