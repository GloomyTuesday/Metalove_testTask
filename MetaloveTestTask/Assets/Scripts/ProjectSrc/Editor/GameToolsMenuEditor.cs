#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System.IO;
using System.Text;
using Scripts.ProjectSrc;

using UnityEngine;

namespace Scripts.ProjectSrc
{
    public static class GameToolsMenuEditor
    {

#if UNITY_EDITOR

        private const string MENU_ROOT = "Game Tools/";
        private const string SCENARIO_FRAMEFILE_DEFAULT_NAME = "Scenario_Frame"; 

        [MenuItem(MENU_ROOT + "New scenario")]
        private static void NewScenarioFrameBtn()
        {
            /*
            string folderPath = EditorUtility.OpenFolderPanel("Select folder to save scenario", Application.dataPath, "");
            if (string.IsNullOrEmpty(folderPath)) return;

            string fileName = EditorUtility.SaveFilePanel("Name your scenario file", folderPath, "NewScenario", "json");
            if (string.IsNullOrEmpty(fileName)) return;
            */

            string path = EditorUtility.SaveFilePanel("Create new scenario frame", "", SCENARIO_FRAMEFILE_DEFAULT_NAME, "json");

            if (string.IsNullOrEmpty(path)) return;

            ScenarioFrameModel scenarioFrame = new ScenarioFrameModel(Application.version);

            string json = JsonUtility.ToJson(scenarioFrame, true);

            File.WriteAllText(path, json, Encoding.UTF8);
            AssetDatabase.Refresh();
        }

        [MenuItem(MENU_ROOT + "Create scenario by copy")]
        private static void CreateScenarioFrameByCopy()
        {
            Debug.Log("Create scenario by copy");
        }

        [MenuItem(MENU_ROOT + "Load scenario frame")]
        private static void LoadScenarioFrame()
        {
            Debug.Log("Load scenario frame");
        }
#endif

    }
}
