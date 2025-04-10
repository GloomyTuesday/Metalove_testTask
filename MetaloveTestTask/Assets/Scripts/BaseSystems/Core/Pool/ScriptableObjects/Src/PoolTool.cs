using System.Collections.Generic;
using UnityEngine;

namespace Scripts.BaseSystems.Pool
{
    [CreateAssetMenu(menuName = "Scriptable Obj/Base systems/Core/Pool/Pool tools")]
    public class PoolTool : ScriptableObject
    {

        public bool Ready { get; private set; }

        //  Number bellow 0 will mean infinit amount 
        Dictionary<PoolObjectTypeId, int> _limitDictionary = new Dictionary<PoolObjectTypeId, int>(); 

        private Transform _activeObjectHolder;
        private Transform _inactiveObjectHolder; 

        public void Initialize(
            Transform activeObjectHolder,
            Transform inactiveObjectHolder,
            PoolObjectLimitInstructionId poolInstruction
            )
        {

        }

        public void SetObjectLimit(PoolObjectTypeId objectTypeId , int maxAmount )
        {
            if (!_limitDictionary.ContainsKey(objectTypeId))
            {
                _limitDictionary.Add(objectTypeId, maxAmount);
                return; 
            }
                
            _limitDictionary[objectTypeId] = maxAmount;
        }
    }
}
