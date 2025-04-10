#if UNITY_EDITOR
using UnityEditor;

namespace Scripts.BaseSystems.Style
{
    [CustomEditor(typeof(TextMeshProStyle))]
    [CanEditMultipleObjects]
    public class TextStyleSrcEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();
            serializedObject.ApplyModifiedProperties();

            foreach (var targetObject in targets)
                if (targetObject is TextMeshProStyle textMeshProStyle)
                    textMeshProStyle.ApplyStyle();
        }
    }
}
#endif