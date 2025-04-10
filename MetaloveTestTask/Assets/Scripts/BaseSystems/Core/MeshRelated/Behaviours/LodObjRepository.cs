using System.Collections.Generic;
using UnityEngine;

namespace Scripts.BaseSystems
{
    public class LodObjRepository : MonoBehaviour
    {

        private List<LODGroup> LODGroupList { get; set; } = new List<LODGroup>();

        private HashSet<LODGroup> ActiveLodGroup { get; set; } = new HashSet<LODGroup>();
        private HashSet<LODGroup> InActiveLodGroup { get; set; } = new HashSet<LODGroup>();

        private int Count { get; set; }
                
        public LODGroup GetFreeLodGroupObj(bool getActivated = true)
        {
            if (InActiveLodGroup.Count < 1)
                return CreateLodGroup(getActivated);

            var lodGroupItem = InActiveLodGroup.GetEnumerator().Current;

            if (getActivated)
            {
                ActivateLodGroupObj(lodGroupItem);
                return lodGroupItem; 
            }

            return lodGroupItem; 
        }

        private LODGroup CreateLodGroup(bool getActivated = true )
        {
            var lodGroupGameObj = new GameObject(Count+" LodGroup");
            Count++;
            lodGroupGameObj.transform.SetParent(transform);
            var lodGroup = lodGroupGameObj.AddComponent<LODGroup>();

            if (getActivated)
            {
                lodGroupGameObj.SetActive(true);
                ActiveLodGroup.Add(lodGroup);
                return lodGroup;
            }

            lodGroupGameObj.SetActive(false);
            InActiveLodGroup.Add(lodGroup);
            return lodGroup;
        }

        public void ActivateLodGroupObj(LODGroup lodGroupObj)
        {
            if (!InActiveLodGroup.Contains(lodGroupObj))
            {
                Debug.LogWarning("\t Collection InActiveLodGroup is missing obj: " + lodGroupObj.name);
                return; 
            }

            InActiveLodGroup.Remove(lodGroupObj);
            ActiveLodGroup.Add(lodGroupObj);
        }

        private void DeactivateLodGroupObj(LODGroup lodGroupObj)
        {
            if (!ActiveLodGroup.Contains(lodGroupObj))
            {
                Debug.LogWarning("\t Collection ActiveLodGroup is missing obj: " + lodGroupObj.name);
                return;
            }

            ActiveLodGroup.Remove(lodGroupObj);
            InActiveLodGroup.Add(lodGroupObj);
        }
    }
}
