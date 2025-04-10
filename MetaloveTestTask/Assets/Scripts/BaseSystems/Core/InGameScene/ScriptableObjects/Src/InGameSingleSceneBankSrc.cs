using Scripts.BaseSystems;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "InGameSingleSceneBank", menuName = "Scriptable Obj/Base systems/Core/In game scene/In game single scene bank")]
public class InGameSingleSceneBankSrc : ScriptableObject, IBankType<GameObject>
{
    [SerializeField]
    private GameObject _sceneToLoadPrefab;
    private GameObject SceneToLoadPrefab 
    {
        get => _sceneToLoadPrefab;
    
        set
        {
            var previousPrefab = _sceneToLoadPrefab;
            
            if (previousPrefab == value) return;

            _sceneToLoadPrefab = value;

            if (value == null)
            {
                if(previousPrefab != null)
                {
                    OnItemRemoved(ScenePrefabInstanceId);
                    return;
                }

                return;
            }

            ScenePrefabInstanceId = _sceneToLoadPrefab.GetInstanceID();
            OnItemAdded(ScenePrefabInstanceId); 
        }
    }
    private int ScenePrefabInstanceId { get; set; }

    public event Action<int> OnItemRemoved;
    public event Action<int> OnItemAdded;

    public void AddItem(GameObject newItem) => SceneToLoadPrefab = newItem;

    public bool ContainsItem(int instanceId) => ScenePrefabInstanceId == instanceId;

    public GameObject[] GetItemArray() => new GameObject[] { SceneToLoadPrefab  };

    public GameObject GetItemByInstanceId(int instanceId) => ScenePrefabInstanceId == instanceId ? SceneToLoadPrefab : null; 

    public void RemoveItem(int instanceId)
    {
        if (ScenePrefabInstanceId == instanceId)
            SceneToLoadPrefab = null; 
    }
}
