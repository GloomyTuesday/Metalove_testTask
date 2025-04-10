#if UNITY_EDITOR
using UnityEditor;

namespace Scripts.BaseSystems.Core
{
    [CustomEditor(typeof(UnitySceneObserver))]
    public class UnitySceneObserverEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var unitySceneObserver = (UnitySceneObserver)target;

            DrawDefaultInspector();

            if (unitySceneObserver.transform.parent != null)
            {
                    unitySceneObserver.transform.SetParent(null);
                    unitySceneObserver.transform.SetSiblingIndex(0);
                    EditorUtility.SetDirty(unitySceneObserver); 
            }
        }
    }
}
#endif