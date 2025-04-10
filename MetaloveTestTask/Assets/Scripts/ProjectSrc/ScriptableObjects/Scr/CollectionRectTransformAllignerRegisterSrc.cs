using Scripts.BaseSystems.UiRelated;
using Scripts.ProjectSrc;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CollectionRectTransformAllignerRegister", menuName = "Scriptable Obj/Project src/Collection rect transform alligner register")]
public class CollectionRectTransformAllignerRegisterSrc : ScriptableObject, ICollectionRegister<IRectTransformAligner>
{
    public Dictionary<int, IRectTransformAligner> _instanceIdItemIndexDictionary;
    public Dictionary<int, IRectTransformAligner> InstanceIdItemIndexDictionary
    {
        get
        {
            if (!_ready)
                Init();

            return _instanceIdItemIndexDictionary;
        }
    }

    public event Action<int, IRectTransformAligner> OnItemRegistered;
    public event Action<int> OnItemUnregistered;

    private List<IRectTransformAligner> _allignerList;
    private List<IRectTransformAligner> AllignerList
    {
        get
        {
            if (!_ready)
                Init();

            return _allignerList;
        }
    }

    [NonSerialized]
    private bool _ready;

    private void Init()
    {
        _instanceIdItemIndexDictionary = new Dictionary<int,IRectTransformAligner>();
        _allignerList = new List<IRectTransformAligner>();

        _ready = true; 
    }

    public IRectTransformAligner GetItemByInstanceId(int instanceId)
    {
        if (!InstanceIdItemIndexDictionary.ContainsKey(instanceId)) return null;

        return InstanceIdItemIndexDictionary[instanceId];
    }

    public IRectTransformAligner[] GetRegistereItems() => AllignerList.ToArray(); 

    public void Register(IRectTransformAligner itemToRegister)
    {
        var instanceId = itemToRegister.InstanceId;

        if (InstanceIdItemIndexDictionary.ContainsKey(instanceId)) return;

        InstanceIdItemIndexDictionary.Add(instanceId, itemToRegister);
        AllignerList.Add(itemToRegister);

        OnItemRegistered?.Invoke(instanceId, itemToRegister);
    }

    public void Unregister(IRectTransformAligner itemToUnregister)
    {
        var instanceId = itemToUnregister.InstanceId;
        Unregister(instanceId); 
    }

    public void Unregister(int instanceId)
    {
        if (!InstanceIdItemIndexDictionary.ContainsKey(instanceId)) return;

        InstanceIdItemIndexDictionary.Remove(instanceId);

        for (int i = 0; i < AllignerList.Count; i++)
        {
            if (AllignerList[i].InstanceId != instanceId) continue;

            AllignerList.RemoveAt(i);
            break;
        }

        OnItemUnregistered?.Invoke(instanceId);
    }
}
