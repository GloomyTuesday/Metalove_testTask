using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.BaseSystems.GameObjListTools
{
    public class GroupAccessIndex : MonoBehaviour, IGroupAccess
    {
        private enum ActiveStateCtrlBehaviour { SetTrue, SetFalse, SetUsed};

        [SerializeField]
        private GroupAccessId _id;
        [SerializeField]
        private ActiveStateCtrlBehaviour _activeStateCtrlBehaviour;
        [SerializeField, Space(10), FilterByType(typeof(IGroupAccessTools))]
        private UnityEngine.Object _groupAccessToolsObj;

        GameObject IGroupAccess.GameObject => gameObject;
        GroupAccessId IGroupAccess.GroupAccessId => _id;

        private bool _ready; 

        private IGroupAccessTools _iGroupAccessTools;
        IGroupAccessTools IGroupAccessTools
        {
            get
            {
                if (_iGroupAccessTools == null)
                    _iGroupAccessTools = _groupAccessToolsObj.GetComponent<IGroupAccessTools>();
                return _iGroupAccessTools; 
            }
        }

        private Dictionary<ActiveStateCtrlBehaviour, Func<bool>> ActiveStateBehaviourDictionary { get; set; }
        
        private void OnEnable()
        {
            if(!_ready)
                Initialization();
        }

        private void OnDestroy()
        {
            IGroupAccessTools.Unregister(this);
        }

        private void Initialization()
        {
            Debug.Log("\t " + gameObject.name + "\t OnEnable() ");
            InitializeActiveStateDictionary();
            IGroupAccessTools.Register(this);
            bool actiState = ActiveStateBehaviourDictionary[_activeStateCtrlBehaviour]();
            IGroupAccessTools.ChangeGroupActiveState(_id, actiState);
            _ready = true; 
        }

        private void InitializeActiveStateDictionary()
        {
            ActiveStateBehaviourDictionary = new Dictionary<ActiveStateCtrlBehaviour, Func<bool>>()
            {
                { ActiveStateCtrlBehaviour.SetTrue, ()=> true},
                { ActiveStateCtrlBehaviour.SetFalse, ()=> false},
                { ActiveStateCtrlBehaviour.SetUsed, () => IGroupAccessTools.GetGroupActiveState(_id) } 
             };
        }
    }
}

