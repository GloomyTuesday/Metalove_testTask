#if UNITY_EDITOR
using UnityEditor;

namespace Scripts.BaseSystems.Style
{
    [CustomEditor(typeof(ImageStyle))]
    [CanEditMultipleObjects]
    public class ImageStyleEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();
            serializedObject.ApplyModifiedProperties();

            foreach (var targetObject in targets)
                if (targetObject is ImageStyle imageStyleø)
                    imageStyleø.ApplyStyle();
        }
    }
}
#endif
