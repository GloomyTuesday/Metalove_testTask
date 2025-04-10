using UnityEngine;

namespace Scripts.BaseSystems.GameObjListTools
{
    public class GameObjListUnit : MonoBehaviour, IGameObjListUnit
    {
        [SerializeField]
        private string key = "";

        [SerializeField]
        private bool _unregisterWhenDisabled;

        [SerializeField,Space(10)]
        private GameObject[] _activeStateContent;
        [SerializeField]
        private GameObject[] _inactiveStateContent;

        [SerializeField, FilterByType(typeof(IGameObjListTools)), Space(10)]
        private UnityEngine.Object _gameObjListTools;

        private IGameObjListTools _iGameObjListTools;
        private IGameObjListTools IGameObjListTools
        {
            get
            {
                if (_iGameObjListTools == null)
                    _iGameObjListTools = _gameObjListTools.GetComponent<IGameObjListTools>();
                return _iGameObjListTools;
            }
        }

        private void OnEnable()
        {
            IGameObjListTools.TryToRegisterActivalable(key, ActivateCallback, DeactivateCallBack, gameObject);
        }

        private void OnDisable()
        {
            if(_unregisterWhenDisabled)
                IGameObjListTools.TryToUnregisterActivalable(key, ActivateCallback, DeactivateCallBack, gameObject);
        }

        private void OnDestroy()
        {
            IGameObjListTools.TryToUnregisterActivalable(key, ActivateCallback, DeactivateCallBack, gameObject);
        }

        private void ActivateCallback()
        {
            SetActiveState(true);
        }

        private void DeactivateCallBack()
        {
            SetActiveState(false);
        }

        private void SetActiveState(bool activeState)
        {
            foreach (var item in _activeStateContent)
                item.SetActive(activeState);

            foreach (var item in _inactiveStateContent)
                item.SetActive(!activeState);
        }

        public void Activate() => IGameObjListTools.Activate(key); 
        public void Deactiate() => IGameObjListTools.Deactiate(key);
    }
}
