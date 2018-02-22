using UnityEngine;
using UnityEditor;
using System.Text.RegularExpressions;

public class LevelNamesSaver {

    [MenuItem("Scenes Info/Save Scenes Names")]
    private static void SaveScenesNames() {
        // First, try to load the list if it already exists
        LevelNamesData levelNames = (LevelNamesData)AssetDatabase.LoadAssetAtPath("Assets/Scenes/LevelNames.asset", typeof(LevelNamesData));

        // Otherwise, create it
        if (levelNames == null) {
            levelNames = ScriptableObject.CreateInstance<LevelNamesData>();
            AssetDatabase.CreateAsset(levelNames, "Assets/Scenes/LevelNames.asset");
        }

        EditorBuildSettingsScene[] buildSettingsScenes = EditorBuildSettings.scenes;
        levelNames.scenesNames.Clear();

        // Fills the list of names in the asset if the scene name matches
        for (int i = 0; i < buildSettingsScenes.Length; i++) {
            Match match = Regex.Match(buildSettingsScenes[i].path, @"([0-9]+-[0-9]+)\.unity");

            if (!match.Success) {
                continue;
            }
            levelNames.scenesNames.Add(match.Groups[1].Value);
        }

        levelNames.ProcessInfos();

        // Writes the unsaved asset changes to disk
        AssetDatabase.SaveAssets();
    }
}
