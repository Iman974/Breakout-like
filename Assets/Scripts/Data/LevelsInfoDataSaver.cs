using UnityEngine;
using UnityEditor;
using System.Text.RegularExpressions;

public class LevelsInfoDataSaver {

    [MenuItem("Scenes Info/Save Scenes Names")]
    private static void SaveScenesNames() {

        // First, try to load the list if it already exists
        string levelsInfoAssetPath = "Assets/Scenes/LevelsInfo.asset";
        LevelsInfoData levelsInfo = AssetDatabase.LoadAssetAtPath<LevelsInfoData>(levelsInfoAssetPath);

        // Otherwise, create it
        if (levelsInfo == null) {
            levelsInfo = ScriptableObject.CreateInstance<LevelsInfoData>();
            AssetDatabase.CreateAsset(levelsInfo, levelsInfoAssetPath);
        }

        EditorBuildSettingsScene[] buildSettingsScenes = EditorBuildSettings.scenes;
        levelsInfo.ResetData();

        // Fills the list of names in the asset if the scene name matches
        for (int i = 0; i < buildSettingsScenes.Length; i++) {
            Match match = RegexUtility.LevelSceneNameMatch(buildSettingsScenes[i].path);

            if (!match.Success) {
                continue;
            }
            string levelDataAssetPath = string.Format("Assets/ScriptableObjects/LevelDatas/{0}.asset", match.Groups[1].Value);
            LevelData levelData = AssetDatabase.LoadAssetAtPath<LevelData>(levelDataAssetPath);
            levelsInfo.AddData(match.Groups[1].Value, buildSettingsScenes[i].path, levelData);
        }
        levelsInfo.ProcessData();
        EditorUtility.SetDirty(levelsInfo);

        // Writes the unsaved asset changes to disk
        AssetDatabase.SaveAssets();
    }
}
