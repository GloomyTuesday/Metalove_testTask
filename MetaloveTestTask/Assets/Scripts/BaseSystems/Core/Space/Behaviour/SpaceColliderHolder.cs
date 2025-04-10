using System.Collections.Generic;
using UnityEngine;

namespace Scripts.BaseSystems.Space
{
    public class SpaceColliderHolder : MonoBehaviour, ISpaceCollider
    {
        [SerializeField]
        private Transform _colliderHolder;

        public Transform Transform => transform;

        private bool UpdateDataSubscribtionFlag { get; set; }
        private int LayerY { get; set; }
        private ISpace3D ISpace3D { get; set; }

        private bool _activeFlag;
        private bool ActiveFlag
        {
            get => _activeFlag;
            set
            {
             //   Debug.Log("\t - SpaceColliderHolder \t _activeFlag: "+ value);
                foreach (var item in ColliderObjectsPoolDictionary[true])
                    item.Value.SetActive(value);

                _activeFlag = value; 
            }
        }

        private int ColliderCountId;
        private string ColliderObjectName = "Collider-";
        //  true - objects currently used
        //  false - objects that are in pool
        //  For second dictionary as key is used object unique Id
        private Dictionary<bool, Dictionary<int, GameObject>> ColliderObjectsPoolDictionary { get; set; } =
            new Dictionary<bool, Dictionary<int, GameObject>>();

        private void OnEnable()
        {
            ColliderObjectsPoolDictionary.Add(true, new Dictionary<int, GameObject>());
            ColliderObjectsPoolDictionary.Add(false, new Dictionary<int, GameObject>());
        }

        public void SetSpaceHolder(ISpace3D iSpace3D)
        {
            if (UpdateDataSubscribtionFlag)
                ISpace3D.OnDataUpdated -= SpaceDataUpdated;

            ISpace3D = iSpace3D;
            ISpace3D.OnDataUpdated += SpaceDataUpdated;
            UpdateDataSubscribtionFlag = true;

            SpaceDataUpdated();
        }

        public void SetActiveLayerYColliders(bool activeFlag, int? layer = null)
        {
            ActiveFlag = activeFlag;

            LayerY = LayerY;
            if (layer != null)
                LayerY = layer.Value;

            SpaceDataUpdated();
        }

        public void SpaceDataUpdated()
        {
            var currentSize = ISpace3D.CurrentSpaceSizeCells;
            var enumerator = ColliderObjectsPoolDictionary[true].GetEnumerator();
            int count = 0;
            GameObject colliderObject;

            for (int x = 0; x < currentSize.X; x++)
            {
                for (int z = 0; z < currentSize.Z; z++)
                {
                    if (count < ColliderObjectsPoolDictionary[true].Count)
                    {
                        enumerator.MoveNext();
                        colliderObject = enumerator.Current.Value;
                    }
                    else
                    {
                        var colliderObjectId = GetFreeColliderObjectUniqueId();
                        SetActiveColliderObject(colliderObjectId, true);
                        colliderObject = ColliderObjectsPoolDictionary[true][colliderObjectId];
                    }

                    var localPosition = ISpace3D.GetLocalPositionByPoint(x, LayerY, z);
                    colliderObject.transform.localPosition = localPosition;
                    count++;
                }
            }

            if (count >= ColliderObjectsPoolDictionary[true].Count) return;

            List<int> elementdToDiactivate = new List<int>();

            while (enumerator.MoveNext())
                elementdToDiactivate.Add(enumerator.Current.Key);

            for (int i = 0; i < elementdToDiactivate.Count; i++)
                SetActiveColliderObject(elementdToDiactivate[i], false);
        }

        private int GetFreeColliderObjectUniqueId()
        {
            if (ColliderObjectsPoolDictionary[false].Count > 0)
            {
                var enumerator = ColliderObjectsPoolDictionary[false].GetEnumerator();
                enumerator.MoveNext();
                return enumerator.Current.Value.GetInstanceID();
            }

            var newColliderObject = new GameObject();
            newColliderObject.layer = _colliderHolder.gameObject.layer; 
            newColliderObject.transform.SetParent(_colliderHolder);
            newColliderObject.AddComponent<BoxCollider>();
            newColliderObject.transform.localScale = ISpace3D.CellSizeAdapted;
            newColliderObject.name = ColliderObjectName + ColliderCountId.ToString();
            ColliderCountId++;
            newColliderObject.SetActive(false);
            int uniqueId = newColliderObject.GetInstanceID();
            ColliderObjectsPoolDictionary[false].Add(uniqueId, newColliderObject);
            return uniqueId;
        }

        private void SetActiveColliderObject(int uniqueId, bool activeState)
        {
            if (!ColliderObjectsPoolDictionary[!activeState].ContainsKey(uniqueId))
            {
                Debug.Log("\t - SpaceColliderHolder \t SetActiveColliderObject \t Object with id: " + uniqueId + "\t" +
                    " is missing from " + (!activeState) + " collection. Could be that object's active state is " + activeState);
                return;
            }

            var colliderObject = ColliderObjectsPoolDictionary[!activeState][uniqueId];
            ColliderObjectsPoolDictionary[activeState].Add(uniqueId, colliderObject);
            ColliderObjectsPoolDictionary[!activeState].Remove(uniqueId);

            if (ActiveFlag && activeState)
                colliderObject.SetActive(activeState);
            else
                colliderObject.SetActive(false);
            
        }

        //  All other object except those from received array are going to be deactivated
        private void SetTheOnlyActiveColliderObjects(int[] colliderObjectsUniqueId)
        {
            Dictionary<int, bool> objectToActivateDictionary = new Dictionary<int, bool>();
            foreach (var item in colliderObjectsUniqueId)
            {
                objectToActivateDictionary.Add(item, true);
            }

            List<int> objectsToDeactivateList = new List<int>();

            foreach (var item in ColliderObjectsPoolDictionary[true])
            {
                if (!objectToActivateDictionary.ContainsKey(item.Key))
                    objectsToDeactivateList.Add(item.Key);
            }

            for (int i = 0; i < objectsToDeactivateList.Count; i++)
                SetActiveColliderObject(objectsToDeactivateList[i], false);

            for (int i = 0; i < colliderObjectsUniqueId.Length; i++)
            {
                int id = colliderObjectsUniqueId[i];
                if (!ColliderObjectsPoolDictionary[true].ContainsKey(id))
                    SetActiveColliderObject(id, true);

            }
        }
    }
}

