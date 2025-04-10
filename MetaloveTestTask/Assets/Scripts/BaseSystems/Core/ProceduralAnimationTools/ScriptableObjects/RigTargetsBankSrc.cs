using System;
using UnityEngine;

namespace Scripts.BaseSystems.ProceduralAnimationTools
{
    [CreateAssetMenu(fileName = "RigTargetsBank", menuName = "Scriptable Obj/Base systems/Core/ProceduralAnimationTools")]
    public class RigTargetsBankSrc : ScriptableObject
    {
        [SerializeField]
        private Transform _lookAtTarget;

        public Transform GetLookAtTarget() => _lookAtTarget;
    }
}
