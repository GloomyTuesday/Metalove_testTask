using Scripts.ProjectSrc;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CollectionRegister", menuName = "Scriptable Obj/Project src/Collection register src")]
public class CollectionRegisterSrc : ScriptableObject, ICollectionRegister<RectTransform>
{
    public event Action<int, RectTransform> OnItemRegistered;
    public event Action<int> OnItemUnregistered;

    private List<RectTransform> _rectTransformList;
    private List<RectTransform> RectTransformList
    {
        get
        {
            if (!_ready)
                Init();

            return _rectTransformList;
        }
    }

    private Dictionary<int, RectTransform> _InstanceIdItemIndexDictionary;
    public Dictionary<int, RectTransform> InstanceIdItemIndexDictionary
    {
        get
        {
            if (!_ready)
                Init();

            return _InstanceIdItemIndexDictionary;
        }
    }

    [NonSerialized]
    private bool _ready;

    private void Init()
    {
        _rectTransformList = new List<RectTransform>();
        _InstanceIdItemIndexDictionary = new Dictionary<int, RectTransform>();

        _ready = true;
    }

    public RectTransform[] GetRegistereItems() => RectTransformList.ToArray();

    public void Register(RectTransform itemToRegister)
    {
        var instanceId = itemToRegister.GetInstanceID();

        if (InstanceIdItemIndexDictionary.ContainsKey(instanceId)) return;

        InstanceIdItemIndexDictionary.Add(instanceId, itemToRegister);
        RectTransformList.Add(itemToRegister);

        OnItemRegistered?.Invoke(instanceId, itemToRegister);
    }

    public void Unregister(int instanceId)
    {
        if (!InstanceIdItemIndexDictionary.ContainsKey(instanceId)) return;

        InstanceIdItemIndexDictionary.Remove(instanceId);

        for (int i = 0; i < RectTransformList.Count; i++)
        {
            if (RectTransformList[i].GetInstanceID() != instanceId) continue;

            RectTransformList.RemoveAt(i);
            break;
        }

        OnItemUnregistered?.Invoke(instanceId);
    }

    public void Unregister(RectTransform itemToUnregister)
    {
        var instanceId = itemToUnregister.GetInstanceID();
        Unregister(instanceId);
    }

    public RectTransform GetItemByInstanceId(int instanceId)
    {
        if (!InstanceIdItemIndexDictionary.ContainsKey(instanceId)) return null;

        return InstanceIdItemIndexDictionary[instanceId];
    }
}
