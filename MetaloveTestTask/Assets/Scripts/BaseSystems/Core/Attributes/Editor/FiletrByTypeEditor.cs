using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Scripts.BaseSystems
{
#if UNITY_EDITOR
    [CustomEditor(typeof(FilterByType))]
    public class FiletrByTypeEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            SerializedProperty filterProperty = serializedObject.GetIterator();
            var obj = filterProperty.objectReferenceValue;

            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}
