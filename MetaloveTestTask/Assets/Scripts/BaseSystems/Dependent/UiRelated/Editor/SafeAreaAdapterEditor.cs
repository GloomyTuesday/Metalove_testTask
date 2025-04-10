#if UNITY_EDITOR
using UnityEditor;

namespace Scripts.BaseSystems.UiRelated
{
    [CustomEditor(typeof(SafeAreaAdapter))]
    public class SafeAreaAdapterEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var safeAreaAdapter = (SafeAreaAdapter)target;

            DrawDefaultInspector();

            if (safeAreaAdapter.transform.parent != null)
            {
                safeAreaAdapter.AdaptRectTransformToSafeArea(safeAreaAdapter.SafeAreaRRectTransform);
                EditorUtility.SetDirty(safeAreaAdapter);
            }
        }
        
    }
}
#endif