using UnityEngine;
using UnityEditor;
using System.Text.RegularExpressions;

public class LevelsInfoSaver {

    [MenuItem("Scenes Info/Save Scenes Info")]
    private static void SaveScenesInfo() {
        EditorBuildSettingsScene[] buildSettingsScenes = EditorBuildSettings.scenes;

        // First, try to load the list if already exists
        LevelsInfo scenesInfo = (LevelsInfo)AssetDatabase.LoadAssetAtPath("Assets/Scenes/ScenesInfo.asset", typeof(LevelsInfo));

        // If doesn't exist, create it !
        if (scenesInfo == null) {
            scenesInfo = ScriptableObject.CreateInstance<LevelsInfo>();
            AssetDatabase.CreateAsset(scenesInfo, "Assets/Scenes/ScenesInfo.asset");
        }

        // Fill the array
        scenesInfo.levelScenesNames = new string[buildSettingsScenes.Length - 1];
        for (int i = 0; i < scenesInfo.levelScenesNames.Length; ++i) {
            scenesInfo.levelScenesNames[i] = Regex.Match(buildSettingsScenes[i + 1].path, @"([0-9]+-[0-9]+)\.unity").Groups[1].Value;
        }

        scenesInfo.ProcessInfos();

        // Writes all unsaved asset changes to disk
        AssetDatabase.SaveAssets();
    }
}
