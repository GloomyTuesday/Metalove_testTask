using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

using System.Collections.Generic;
using UnityEngine.SceneManagement;


namespace Scripts.BaseSystems
{
  #if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(FilterByType))]
    public class FilterByTypeDrawer : PropertyDrawer
    {
        private enum AvailableType { Non, ScriptableObject, GameObject, UnityObject };

        [SerializeField]
        private System.Type _lastSelectedObjType;

        private Vector2 _btnSize = new Vector2(70, 20);
        private bool ShowPopup { get; set; }
        private Object SelectedObj { get; set; }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
                        
            SelectedObj = property.serializedObject.targetObject;
                        
            var obj = property.objectReferenceValue;

            Rect buttonRect = new Rect(position.xMax - _btnSize.x, position.y, _btnSize.x, _btnSize.y);

            if (GUI.Button(buttonRect, "Obj list"))
            {
                ShowPopup = true;
            }

            if (ShowPopup)
            {
                var attributeData = (FilterByType)attribute;
                var filterType = attributeData.FilterType;
                Object[] objArray;
                string[] objNameArray;
                AvailableType[] typeHolder;

                var resultData = GetAvailableObjectCollectionsAndRestoreSelectedObjectIndex(filterType, property);

                objArray = resultData.Item1;
                objNameArray = resultData.Item2;
                typeHolder = resultData.Item3;

                GenericMenu menu = new GenericMenu();

                menu.AddItem(
                            new GUIContent("Cleare"),
                            obj == null,
                            () => {
                                OnItemSelected(null, property);
                            }
                        );

                for (int i = 0; i < objNameArray.Length; i++)
                {
                    var castedObj = objArray[i];
                    if (castedObj == null) continue;

                    int index = i;
                    menu.AddItem(
                            new GUIContent(objNameArray[i]),
                            obj == castedObj,
                            () => {
                                OnItemSelected(castedObj, property);
                            }
                        );
                }

                menu.DropDown(buttonRect);
                ShowPopup = false;
            }

            var objectFieldPosition = position;
            objectFieldPosition.x -= _btnSize.x;


            EditorGUI.ObjectField(objectFieldPosition, label.text = "\t\t" + label.text, obj, _lastSelectedObjType, true);
         //   EditorGUI.PropertyField(objectFieldPosition, property, GUIContent.none);
            EditorGUI.EndProperty();
        }   

        private (Object[], string[], AvailableType[]) GetAvailableObjectCollectionsAndRestoreSelectedObjectIndex(
            System.Type filterType,
            SerializedProperty property
            )
        {
            var resultObjList = new List<Object>() { null };
            var resultObjNameList = new List<string>() { "Empty" };
            var typeHolderList = new List<AvailableType>() { AvailableType.Non };

            var listWithUnverifiedObjectsFromAssets = new List<string>();

            var objType = GetObjType();

            switch (objType)
            {
                case AvailableType.GameObject:
                    {
                        listWithUnverifiedObjectsFromAssets.AddRange(
                        AssetDatabase.FindAssets("t:Prefab", new[] { "Assets" })
                        );

                        //  Adding object from scene 

                        var objectFromSceneSearchResult = GetAllGameObjectsWithTypeFromScene(filterType);
                        var objCollection = objectFromSceneSearchResult.Item1;
                        var nameCollection = objectFromSceneSearchResult.Item2;
                        var typeHolderCollection = objectFromSceneSearchResult.Item3;
                        resultObjList.AddRange(objCollection);
                        resultObjNameList.AddRange(nameCollection);
                        typeHolderList.AddRange(typeHolderCollection);
                    }
                    break;

                case AvailableType.ScriptableObject:
                    {
                        listWithUnverifiedObjectsFromAssets.AddRange(
                        AssetDatabase.FindAssets("t:ScriptableObject", new[] { "Assets" })
                        );
                    }
                    break;

                case AvailableType.UnityObject:
                    {
                        listWithUnverifiedObjectsFromAssets.AddRange(
                        AssetDatabase.FindAssets("t:ScriptableObject", new[] { "Assets" })
                        );

                        listWithUnverifiedObjectsFromAssets.AddRange(
                        AssetDatabase.FindAssets("t:Prefab", new[] { "Assets" })
                        );

                        //  Adding object from scene 

                        var objectFromSceneSearchResult = GetAllGameObjectsWithTypeFromScene(filterType);
                        var objCollection = objectFromSceneSearchResult.Item1;
                        var nameCollection = objectFromSceneSearchResult.Item2;
                        var typeHolderCollection = objectFromSceneSearchResult.Item3;
                        resultObjList.AddRange(objCollection);
                        resultObjNameList.AddRange(nameCollection);
                        typeHolderList.AddRange(typeHolderCollection);
                    }
                    break;
            }

            for (int i = 0; i < listWithUnverifiedObjectsFromAssets.Count; i++)
            {
                var assetGUID = listWithUnverifiedObjectsFromAssets[i];
                string assetPath = AssetDatabase.GUIDToAssetPath(assetGUID);

                switch (objType)
                {
                    case AvailableType.GameObject:
                        {
                            Object obj = AssetDatabase.LoadAssetAtPath<Object>(assetPath);
                            if (obj != null)
                            {
                                var gameObject = obj as GameObject;

                                if (gameObject == null)
                                    break;

                                if (gameObject.GetComponent(filterType) != null)
                                {
                                    resultObjList.Add(gameObject);
                                    resultObjNameList.Add(gameObject.name);
                                }
                            }
                        }
                        break;

                    case AvailableType.ScriptableObject:
                        {
                            ScriptableObject scriptObj = AssetDatabase.LoadAssetAtPath<ScriptableObject>(assetPath);
                            if (scriptObj != null && filterType.IsAssignableFrom(scriptObj.GetType()))
                            {
                                resultObjList.Add(scriptObj);
                                resultObjNameList.Add(scriptObj.name);
                            }
                        }
                        break;

                    case AvailableType.UnityObject:
                        {
                            ScriptableObject scriptObj = AssetDatabase.LoadAssetAtPath<ScriptableObject>(assetPath);
                            if (scriptObj != null && filterType.IsAssignableFrom(scriptObj.GetType()))
                            {
                                resultObjList.Add(scriptObj);
                                resultObjNameList.Add(scriptObj.name);
                                continue;
                            }

                            Object obj = AssetDatabase.LoadAssetAtPath<Object>(assetPath);
                            if (obj == null) break;

                            var gameObject = obj as GameObject;

                            if (gameObject == null) break;

                            if (filterType.IsAssignableFrom(obj.GetType()))
                            {
                                resultObjList.Add(gameObject);
                                resultObjNameList.Add(gameObject.name);
                            }
                        }
                        break;
                }
            }

            return (resultObjList.ToArray(), resultObjNameList.ToArray(), typeHolderList.ToArray());
        }

        private (Object[], string[], AvailableType[]) GetAllGameObjectsWithTypeFromScene(System.Type filterType)
        {

            var objectList = new List<Object>();
            var nameList = new List<string>();
            var typeHolderList = new List<AvailableType>();

            var gameObjectCollection = new List<GameObject>();
            var instanceIdGameObjectDictionary = new Dictionary<int, GameObject>(); //  used to prevent repeating elements in collection
            var currentScene = SceneManager.GetActiveScene();
            var rootGameObjects = currentScene.GetRootGameObjects();

            for (int i = 0; i < rootGameObjects.Length; i++)
            {
                CoompleteCollectionWithChildUniqueGameObj(rootGameObjects[i], gameObjectCollection, instanceIdGameObjectDictionary);
            }

            var selectedObj = Selection.gameObjects;
            for (int i = 0; i < selectedObj.Length; i++)
            {
                CoompleteCollectionWithChildUniqueGameObj(selectedObj[i], gameObjectCollection, instanceIdGameObjectDictionary);
            }

            foreach (var item in gameObjectCollection)
            {
                var componentsCollection = item.GetComponents<Component>();

                foreach (var component in componentsCollection)
                {
                    if (component == null) continue;

                    if (filterType.IsAssignableFrom(component.GetType()))
                    {
                        var gameObj = item;
                        objectList.Add(gameObj);
                        nameList.Add(gameObj.name + "\t Scene");
                        break;
                    }
                }
            }

            return (objectList.ToArray(), nameList.ToArray(), typeHolderList.ToArray());
        }

        private void CoompleteCollectionWithChildUniqueGameObj(GameObject parent, List<GameObject> goList, Dictionary<int, GameObject> registeredObjDictionary = null)
        {
            var instanceId = parent.GetInstanceID();

            if (registeredObjDictionary != null)
            {
                if (registeredObjDictionary.ContainsKey(instanceId)) return;

                registeredObjDictionary.Add(instanceId, parent);
            }
            else
            {
                for (int i = 0; i < goList.Count; i++)
                    if (goList[i].GetInstanceID() == instanceId) return;
            }

            goList.Add(parent);

            var childCount = parent.transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                var child = parent.transform.GetChild(i);
                CoompleteCollectionWithChildUniqueGameObj(child.gameObject, goList, registeredObjDictionary);
            }
        }

        private AvailableType GetObjType()
        {
            var objType = fieldInfo.FieldType;

            if (typeof(GameObject).IsAssignableFrom(objType))
                return AvailableType.GameObject;

            if (typeof(ScriptableObject).IsAssignableFrom(objType))
                return AvailableType.ScriptableObject;

            if (typeof(Object).IsAssignableFrom(objType))
                return AvailableType.UnityObject;

            return AvailableType.Non;
        }

        private void OnItemSelected(Object selectedObj, SerializedProperty property)
        {
            property.serializedObject.Update();
            property.objectReferenceValue = selectedObj;
            property.serializedObject.ApplyModifiedProperties();

            if (selectedObj == null)
                _lastSelectedObjType = null;
            else
                _lastSelectedObjType = selectedObj.GetType();

            EditorUtility.SetDirty(SelectedObj);
            EditorSceneManager.MarkAllScenesDirty();
        }
    }
  #endif

}
