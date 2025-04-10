#if UNITY_EDITOR
using UnityEditor;

namespace Scripts.BaseSystems.Localization
{
    [CustomEditor(typeof(LocalizatorText))]
    [CanEditMultipleObjects]
    public class LocalizatorTextEditor : Editor
    {
        //  
        SerializedProperty _update;

        //  Export source fields
        SerializedProperty _localizationKey;

        SerializedProperty _initValue;

        SerializedProperty _exportSourceId;

        SerializedProperty _textMeshPro;
        SerializedProperty _textMeshProCollection;
        SerializedProperty _tmpInputField;
        SerializedProperty _tmpInputFieldCollection;

        //  Import source fields
        SerializedProperty _importSourceId;

        SerializedProperty _importSourceScriptableObject;
        SerializedProperty _localizationDataSourceObj;

        void OnEnable()
        {
            _update = serializedObject.FindProperty("_update");

            //  Export source fields
            _localizationKey = serializedObject.FindProperty("_localizationKey");

            _initValue = serializedObject.FindProperty("_initValue");

            _exportSourceId = serializedObject.FindProperty("_exportSourceId");

            _textMeshPro = serializedObject.FindProperty("_textMeshPro");
            _textMeshProCollection = serializedObject.FindProperty("_textMeshProCollection");
            _tmpInputField = serializedObject.FindProperty("_tmpInputField");
            _tmpInputFieldCollection = serializedObject.FindProperty("_tmpInputFieldCollection");

            //  Import source fields
            _importSourceId = serializedObject.FindProperty("_importSourceId");

            _importSourceScriptableObject = serializedObject.FindProperty("_importSourceScriptableObject");
            _localizationDataSourceObj = serializedObject.FindProperty("_localizationDataSourceObj");
        }

        public override void OnInspectorGUI()
        {
            using (new EditorGUI.DisabledScope(true))
                EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"));

            EditorGUILayout.PropertyField(_update);
            EditorGUILayout.PropertyField(_localizationKey);
            EditorGUILayout.PropertyField(_initValue);
            
            EditorGUILayout.PropertyField(_exportSourceId);
            ExportLocalizationSourceId exportSourceId = (ExportLocalizationSourceId)_exportSourceId.enumValueIndex;

            switch (exportSourceId)
            {
                case ExportLocalizationSourceId.Non:
                    break;
                case ExportLocalizationSourceId.TextMeshProUGUI:
                    EditorGUILayout.PropertyField(_textMeshPro);
                    break;
                case ExportLocalizationSourceId.TextMeshProUGUI_collection:
                    EditorGUILayout.PropertyField(_textMeshProCollection);
                    break;
                case ExportLocalizationSourceId.TMP_InputField:
                    EditorGUILayout.PropertyField(_tmpInputField);
                    break;
                case ExportLocalizationSourceId.TMP_InputField_collection:
                    EditorGUILayout.PropertyField(_tmpInputFieldCollection);
                    break;
                default:
                    break;
            }

            EditorGUILayout.PropertyField(_importSourceId);
            ImportLocalizationSourceId importSourceId = (ImportLocalizationSourceId)_importSourceId.enumValueIndex;

            switch (importSourceId)
            {
                case ImportLocalizationSourceId.Non:
                    break;
                case ImportLocalizationSourceId.Custom_ScriptableObj:
                    EditorGUILayout.PropertyField(_importSourceScriptableObject);
                    break;
                case ImportLocalizationSourceId.FilteredByType_ITextLocalizator:
                    EditorGUILayout.PropertyField(_localizationDataSourceObj);
                    break;
                default:
                    break;
            }

            serializedObject.ApplyModifiedProperties();
        }
        
    }
}
#endif
