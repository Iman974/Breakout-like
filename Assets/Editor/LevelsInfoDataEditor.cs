using UnityEditor;

[CustomEditor(typeof(LevelsInfoData))]
public class LevelsInfoDataEditor : Editor {

    private static bool[][] areLevelsFoldout;
    private LevelsInfoData levelsInfo;

    private void OnEnable() {
        levelsInfo = (LevelsInfoData)target;

        if (areLevelsFoldout != null) {
            return;
        }

        areLevelsFoldout = new bool[levelsInfo.Worlds.Length][];
        for (int i = 0; i < areLevelsFoldout.Length; i++) {
            areLevelsFoldout[i] = new bool[levelsInfo.Worlds[i].LevelCount];
        }
    }

    public override void OnInspectorGUI() {
        if (levelsInfo == null) {
            return;
        }

        int worldIndex = 0;

        foreach (var world in levelsInfo.Worlds) {
            EditorGUILayout.LabelField("World " + (worldIndex + 1), GUIStyleUtility.boldFont);

            for (int i = 0; i < world.LevelCount; i++) {
                EditorGUILayout.BeginVertical(GUIStyleUtility.foldoutPadding);

                areLevelsFoldout[worldIndex][i] = EditorGUILayout.Foldout(areLevelsFoldout[worldIndex][i], "Level " + (i + 1), true);

                if (areLevelsFoldout[worldIndex][i]) {
                    EditorGUI.indentLevel += 1;

                    EditorGUILayout.LabelField("Name: " + world[i].Name);
                    EditorGUILayout.LabelField("Scene Build Index: " + world[i].SceneIndex);
                    EditorGUILayout.LabelField("Level Data: " + (world[i].Data != null ? "Found" : "Missing"));

                    EditorGUI.indentLevel -= 1;
                }

                EditorGUILayout.EndVertical();
            }
            worldIndex++;
        }
    }
}
