using UnityEngine;

namespace Scripts.BaseSystems
{
    [CreateAssetMenu
        (
        fileName = "ThreadTools",
        menuName = "Scriptable Obj/Base systems/Core/Thread tools"
        )
    ]
    public class ThreadToolsSrc : ScriptableObject, IThreadTools
    {
        public bool IIsMultiThreadSupported
        {
            get
            {
#if UNITY_WEBGL
                return false ;
#endif
                return true; 
            }
        }
    }
}
