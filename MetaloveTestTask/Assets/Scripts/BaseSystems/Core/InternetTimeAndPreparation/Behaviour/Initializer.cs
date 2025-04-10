using UnityEngine;

namespace Scripts.BaseSystems.Core
{
    public class Initializer : MonoBehaviour
    {
        [SerializeField]
        private bool _rearrange;

        [SerializeField, Space(10)]
        private GameObject[] _iReadyOwners;

        [SerializeField, Space(10), Header("Time to wait until all objects will be ready")]
        private int _millisecondsToWait = 1000;
        [SerializeField, FilterByType(typeof(IWaitAndLaunchTool)), Space(10)]
        private WaitAndLaunchToolSrc _waitAndLaunchToolObj;

        private IWaitAndLaunchTool _iWaitAndLaunchTool;
        private IWaitAndLaunchTool IWaitAndLaunchTool
        {
            get
            {
                if (_iWaitAndLaunchTool == null)
                    _iWaitAndLaunchTool = _waitAndLaunchToolObj.GetComponent<IWaitAndLaunchTool>();

                return _iWaitAndLaunchTool;
            }
        }
        
        private void OnValidate()
        {
            if (_rearrange)
            {
                _rearrange = false;

                int siblingIndexOffset = 0;

                for (int i = 0; i < _iReadyOwners.Length; i++)
                {
                    if (_iReadyOwners[i].transform.parent == transform)
                    {
                        Debug.Log("\t " + _iReadyOwners[i].name + "\t index: " + i);
                        _iReadyOwners[i].transform.SetSiblingIndex(i - siblingIndexOffset);
                    }
                    else
                    {
                        siblingIndexOffset++;
                    }
                }
            }

            if(_iReadyOwners!=null)
                foreach (var item in _iReadyOwners)
                    item.SetActive(false); 
        }

        private void OnEnable()
        {
            Initialize();
        }

        private async void Initialize()
        {
            if (_iReadyOwners == null) return; 

            foreach (var item in _iReadyOwners)
                item.SetActive(false);

            for (int i = 0; i < _iReadyOwners.Length; i++)
            {
                Activate(_iReadyOwners[i]);
                Debug.Log("Start initializing: " + _iReadyOwners[i].name);

                await IWaitAndLaunchTool.WaitAndLaunch(_iReadyOwners[i], gameObject, _millisecondsToWait);

                Debug.Log("Finish initializing: "+ _iReadyOwners[i].name); 
            }
        }

        private void Activate(GameObject obj)
        {
            obj.SetActive(true);
            var parent = obj.transform;

            while (parent != null)
            {
                if (parent == null) return;

                parent.gameObject.SetActive(true);
                parent = parent.transform.parent;
            }
        }
    }
}

