using UnityEngine;
using System;
using System.Collections.Generic;

namespace Scripts
{
    public class InsideContentInstantiator : MonoBehaviour
    {
        [SerializeField]
        private ContentUnit[] _content; 

        [Serializable]
        private struct ContentUnit
        {
            public GameObject[] _contentToInstantiate;
            public GameObject[] _contentHolders;
        }

        [SerializeField,HideInInspector]
        private GameObject[] _instantiatedContent;

        private void OnEnable()
        {
            DestroyAllContent();
            _instantiatedContent = InstantiateContent(_content);
        }

        private GameObject[] InstantiateContent(ContentUnit[] content)
        {
            List<GameObject> instantiatedContent = new List<GameObject>();

            for (int i = 0; i < content.Length; i++)
            {
                for (int j = 0; j < content[i]._contentHolders.Length; j++)
                {
                    var contentHolder = content[i]._contentHolders[j]; 

                    for (int k = 0; k < content[i]._contentToInstantiate.Length; k++)
                    {
                        var contentToInstantiate = content[i]._contentToInstantiate[k];
                        var gameObj = Instantiate(contentToInstantiate, contentHolder.transform);
                        /*
                        gameObj.transform.localScale = Vector3.one;
                        gameObj.transform.localPosition = Vector3.zero;
                        */
                        instantiatedContent.Add(gameObj); 
                    }
                }
            }

            return instantiatedContent.ToArray(); 
        }

        private void DestroyAllContent()
        {
            for (int i = _instantiatedContent.Length - 1; i >= 0; i--)
                DestroyImmediate(_instantiatedContent[i]);

            _instantiatedContent = null;

            for (int i = 0; i < _content.Length; i++)
            {
                for (int j = 0; j < _content[i]._contentHolders.Length; j++)
                {
                    var contentHolderTransform = _content[i]._contentHolders[j].transform;
                    int childAmount = contentHolderTransform.childCount;
                    if (childAmount < 1) continue; 

                    for (int k = childAmount-1; k >=0; k--)
                        DestroyImmediate(contentHolderTransform.GetChild(k));
                }
            }
        }
    }
}
