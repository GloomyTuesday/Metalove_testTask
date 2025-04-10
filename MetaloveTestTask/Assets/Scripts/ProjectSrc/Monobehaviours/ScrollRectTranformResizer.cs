using Scripts.BaseSystems;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.GridLayoutGroup;

namespace Scripts.ProjectSrc
{
    public class ScrollRectTranformResizer : MonoBehaviour
    {
        private enum ResizeAlgorithm { IncreaseVerital_and_BiggetHorizontl}

        [SerializeField]
        private ResizeAlgorithm _resizeAlgorithm;


        [SerializeField]
        [Uneditable]
        private RectTransform _rectTransform;
        private RectTransform RectTransform { get => _rectTransform; set => _rectTransform = value;  }

        [SerializeField]
        [FilterByType(typeof(ICollectionRegister<RectTransform>))]
        private Object _collectionRegisterObj;

        private ICollectionRegister<RectTransform> _icollectionRegisterObj;
        private ICollectionRegister<RectTransform> ICollectionRegisterObj
        {
            get
            {
                if (_icollectionRegisterObj == null)
                    _icollectionRegisterObj = _collectionRegisterObj.GetComponent<ICollectionRegister<RectTransform>>();

                return _icollectionRegisterObj;
            }
        }

        private Dictionary<int, RectTransform> RectTransformDictionary { get; set; } = new Dictionary<int, RectTransform>(); 

        private void OnValidate()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        private void OnEnable()
        {
            Subscribe(); 
        }

        private void OnDisable()
        {
            Unsubscribe(); 
        }

        private void Subscribe()
        {
            ICollectionRegisterObj.OnItemRegistered += ItemRegistered;
            ICollectionRegisterObj.OnItemUnregistered += ItemUnregistered;
        }

        private void Unsubscribe()
        {
            ICollectionRegisterObj.OnItemRegistered -= ItemRegistered;
            ICollectionRegisterObj.OnItemUnregistered -= ItemUnregistered;
        }

        private void ItemRegistered(int instanceId, RectTransform transform)
        {
            if (RectTransformDictionary.ContainsKey(instanceId)) return;

            RectTransformDictionary.Add(instanceId, transform);

            RecalcRectTransformSize(); 
        }

        private void ItemUnregistered(int instanceId)
        {
            if (!RectTransformDictionary.ContainsKey(instanceId)) return;

            RecalcRectTransformSize();
        }

        private void RecalcRectTransformSize()
        {
            Vector2 rectSize = Vector2.zero;

            switch (_resizeAlgorithm)
            {
                case ResizeAlgorithm.IncreaseVerital_and_BiggetHorizontl:

                    var vectorBuffer = Vector2.zero;
                    int count = 0;
                    Vector3[] corners = new Vector3[4];
                    float width = 0;
                    float height = 0; 

                    foreach (var item in RectTransformDictionary)
                    {
                        if (rectSize.x < item.Value.rect.width)
                            rectSize.x = item.Value.rect.width;

                        Debug.Log("\t\t [ "+count+" ] " + item.Value.rect.width + " : "+ height + "\t "+ item.Value.name);
                        count++; 
                        rectSize.y += item.Value.rect.height;
                    }

                    break; 
            }

            RectTransform.sizeDelta.Set( rectSize.x, rectSize.y );

            Debug.Log("\t new rect size: "+ RectTransform.sizeDelta+"\t "+ rectSize); 
            LayoutRebuilder.ForceRebuildLayoutImmediate(RectTransform);
        }
    }
}
