using UnityEngine;

namespace Scripts.BaseSystems
{
    [CreateAssetMenu( fileName = "MemoryTools", menuName = "Scriptable Obj/Base systems/Core/Memory tools")]
    public class MemoryToolsSrc : ScriptableObject, IMemoryTools
    {
        public float GetUsedMemory
        {
            get
            {
                var memoryInBytes = System.GC.GetTotalMemory(false);
                return memoryInBytes / (1024f * 1024f);
            }
        }

        public void GCCollect() => System.GC.Collect();

        public void GCCollectAndPrintUsedMemory()
        {
            GCCollect();
            Debug.Log("\t Used memory: "+ GetUsedMemory+" Mb"); 
        }
    }
}
