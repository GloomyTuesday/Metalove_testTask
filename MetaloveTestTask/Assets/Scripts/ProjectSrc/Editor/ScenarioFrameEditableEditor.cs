#if UNITY_EDITOR
using System.IO;
using System.Text;
using UnityEditor;
#endif

using UnityEngine;

namespace Scripts.ProjectSrc
{
#if UNITY_EDITOR
    [CustomEditor(typeof(ScenarioFrameEditableSrc))]
    public class ScenarioFrameEditableEditor : Editor
    {
        private const float WIDTH = 173;
        private const float WIDTH_SMALL = 85;
        private const float HEIGHT = 25;

        private const string SCENARIO_FRAMEFILE_DEFAULT_NAME = "Scenario_Frame";

        private string _lastUsedFilePath = "";
        private string LastUsedFilePath
        {
            get => _lastUsedFilePath; 
            set
            {
                _lastUsedFilePath = value;
            }
        }

        private string _lastUsedFileName = "";
        private string LastUsedFileName
        {
            get => _lastUsedFileName;
            set
            {
                _lastUsedFileName = value;
            }
        }

        public override void OnInspectorGUI()
        {
            ScenarioFrameEditableSrc scenarioFrameEditable = (ScenarioFrameEditableSrc)target;

            EditorGUILayout.Space(10);

            EditorGUILayout.LabelField("Works only in play mode");

            //  ----------------------------------------    BeginHorizontal
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Apply current scenario frame", GUILayout.Width(WIDTH), GUILayout.Height(HEIGHT)) && Application.isPlaying)
            {

            }
            EditorGUILayout.EndHorizontal();
            //  ----------------------------------------    EndHorizontal

            EditorGUILayout.Space(10);

            //  ----------------------------------------    BeginHorizontal
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Create new", GUILayout.Width(WIDTH), GUILayout.Height(HEIGHT)))
            {
                string newPath = EditorUtility.SaveFilePanel
                    (
                    "Create new scenario frame",
                    LastUsedFilePath,
                    SCENARIO_FRAMEFILE_DEFAULT_NAME,
                    ""
                    );

                if (!Path.HasExtension(newPath))
                    newPath += ScenarioFrameEditableSrc.EXTENSION;

                if (!string.IsNullOrEmpty(newPath))
                {
                    ScenarioFrameModel scenarioFrame = new ScenarioFrameModel(Application.version);

                    var fileName = Path.GetFileNameWithoutExtension(newPath);
                    scenarioFrame._scenarioFrameId = fileName;

                    string json = JsonUtility.ToJson(scenarioFrame, true);

                    File.WriteAllText(newPath, json, Encoding.UTF8);
                    AssetDatabase.Refresh();

                    scenarioFrameEditable.ImportData(scenarioFrame);
                    LastUsedFilePath = Path.GetDirectoryName(newPath);
                    scenarioFrameEditable.Path = newPath;
                }
                
            }
                        
            EditorGUILayout.EndHorizontal();
            //  ----------------------------------------    EndHorizontal

            EditorGUILayout.Space(10);

            //  ----------------------------------------    BeginHorizontal
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Save as", GUILayout.Width(WIDTH), GUILayout.Height(HEIGHT)))
            {
                if (!scenarioFrameEditable.IsScenarioFrameAvailable) return; 

                string newPath = EditorUtility.SaveFilePanel
                    (
                    "Save scenario frame as",
                    LastUsedFilePath,
                    LastUsedFileName,
                    ""
                    );

                if (string.IsNullOrEmpty(newPath)) return;

                if (!Path.HasExtension(newPath))
                    newPath += ScenarioFrameEditableSrc.EXTENSION;

                try
                {
                    var scenarioFrame = scenarioFrameEditable.ScenarioFrame;
                    string json = JsonUtility.ToJson(scenarioFrame, true);
                    File.WriteAllText(newPath, json, Encoding.UTF8);
                    LastUsedFilePath = Path.GetDirectoryName(newPath);
                    scenarioFrameEditable.Path = newPath;
                }
                catch (System.Exception ex)
                {
                    Debug.LogError(ex.Message);
                }
            }

            if (GUILayout.Button("Load", GUILayout.Width(WIDTH), GUILayout.Height(HEIGHT)))
            {
                string newPath = EditorUtility.OpenFilePanel
                    (
                    "",
                    LastUsedFilePath,
                    ""
                    );

                if (!string.IsNullOrEmpty(newPath))
                {
                    try
                    {
                        string json = File.ReadAllText(newPath, Encoding.UTF8);
                        var scenarioFrame = JsonUtility.FromJson<ScenarioFrameModel>(json);

                        Debug.Log("full load path: \t "+ newPath); 
                        var fileName = Path.GetFileNameWithoutExtension(newPath);
                        LastUsedFileName = fileName;
                        var directory = Path.GetDirectoryName(newPath);
                        scenarioFrame._scenarioFrameId = fileName;

                        scenarioFrameEditable.ImportData(scenarioFrame);
                        scenarioFrameEditable.Path = newPath;
                        
                        LastUsedFilePath = directory;
                    }
                    catch (System.Exception ex)
                    {
                        Debug.LogError($"Failed to load scenario frame: {ex.Message}");
                    }
                }

            }
            EditorGUILayout.EndHorizontal();
            //  ----------------------------------------    EndHorizontal

            //  ----------------------------------------    BeginHorizontal
            EditorGUILayout.BeginHorizontal();

            
            if (GUILayout.Button("Save", GUILayout.Width(WIDTH), GUILayout.Height(HEIGHT)))
            {
                scenarioFrameEditable.SaveCurrent();

                string fullPath = scenarioFrameEditable.Path;
                if (!string.IsNullOrEmpty(fullPath))
                {
                    string assetPath = fullPath.Replace(Application.dataPath, "Assets").Replace("\\", "/");
                    string directory = Path.GetDirectoryName(assetPath).Replace("\\", "/");

                    Debug.Log("\t Reimport: \t asset path: "+ assetPath+ "\t directory: "+ directory);
                    AssetDatabase.ImportAsset(directory, ImportAssetOptions.ForceUpdate);
                    AssetDatabase.Refresh(); 
                }
            }
            
            EditorGUILayout.EndHorizontal();
            //  ----------------------------------------    EndHorizontal

            EditorGUILayout.Space(10);

            //  ----------------------------------------    BeginHorizontal
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Clear", GUILayout.Width(WIDTH), GUILayout.Height(HEIGHT)))
            {
                scenarioFrameEditable.Clear(); 
            }
            EditorGUILayout.EndHorizontal();
            //  ----------------------------------------    EndHorizontal

            EditorGUILayout.Space(10);
            //   EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            // Draw default inspector below buttons
            base.OnInspectorGUI();
        }
    }
#endif
}
