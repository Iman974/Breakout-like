using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelsInfoData))]
public class LevelsInfoDataEditor : Editor {

    private static bool[] areLevelsFoldout = new bool[5];

    public override void OnInspectorGUI() {
        LevelsInfoData levelsInfo = (LevelsInfoData)target;

        if (levelsInfo == null) {
            return;
        }

        int worldIndex = 0;
        foreach (var world in levelsInfo.worlds) {
            for (int i = 0; i < world.levels.Count; i++) {
                EditorGUILayout.LabelField("World " + (worldIndex + 1), GUIStyleUtility.boldFont);

                EditorGUILayout.BeginVertical(GUIStyleUtility.foldoutPadding);

                areLevelsFoldout[i] = EditorGUILayout.Foldout(areLevelsFoldout[i], "Level " + (i + 1), true);

                if (areLevelsFoldout[i]) {
                    EditorGUI.indentLevel += 1;

                    EditorGUILayout.LabelField("Name: " + world.levels[i].levelName);
                    EditorGUILayout.LabelField("Scene Index: " + world.levels[0].sceneIndex.ToString());
                    EditorGUILayout.LabelField("Level Data: " + (world.levels[i].levelData != null ? "Found" : "Not found"));

                    EditorGUI.indentLevel -= 1;
                }

                EditorGUILayout.EndVertical();
            }
            worldIndex++;
        }
    }
}
