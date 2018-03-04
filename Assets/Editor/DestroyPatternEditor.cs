using UnityEditor;

[CustomEditor(typeof(DestroyPattern))]
public class DestroyPatternEditor : Editor {

    private SerializedProperty brickVectorArray;
    private SerializedProperty[] areVectorsEnabled;
    private DestroyPattern pattern;

    private void OnEnable() {
        brickVectorArray = serializedObject.FindProperty("brickVectors");
        areVectorsEnabled = new SerializedProperty[brickVectorArray.arraySize];

        for (int i = 0; i < areVectorsEnabled.Length; i++) {
            areVectorsEnabled[i] = brickVectorArray.GetArrayElementAtIndex(i).FindPropertyRelative("enabled");
        }

        pattern = (DestroyPattern)target;
    }

    public override void OnInspectorGUI() {
        if (pattern == null || brickVectorArray == null) {
            return;
        }

        serializedObject.Update();
        for (int i = 0; i < pattern.BrickVectors.Length; i++) {
            EditorGUILayout.BeginHorizontal();

            areVectorsEnabled[i].boolValue = EditorGUILayout.ToggleLeft(pattern.BrickVectors[i].Name, areVectorsEnabled[i].boolValue,
                GUIStyleUtility.leftToggleLabelStyle);

            EditorGUILayout.EndHorizontal();

            if (!pattern.BrickVectors[i].Enabled) {
                continue;
            }
            EditorGUILayout.BeginVertical(GUIStyleUtility.rightPadding2);

            SerializedProperty currentSegment = brickVectorArray.GetArrayElementAtIndex(i);
            EditorGUILayout.LabelField("Direction", pattern.BrickVectors[i].Direction.ToString());
            EditorGUILayout.PropertyField(currentSegment.FindPropertyRelative("count"));
            EditorGUILayout.PropertyField(currentSegment.FindPropertyRelative("length"));

            EditorGUILayout.EndVertical(); 
        }
        serializedObject.ApplyModifiedProperties();
    }

    public override bool RequiresConstantRepaint() {
        if (pattern == null) {
            return base.RequiresConstantRepaint();
        }

        for (int i = 0; i < pattern.BrickVectors.Length; i++) {
            if (!pattern.BrickVectors[i].Enabled) {
                continue;
            }

            int previousVectorIndex = i - 1;
            while (previousVectorIndex >= 0 && !pattern.BrickVectors[previousVectorIndex].Enabled) {
                DestroyPattern.BrickVector previousSegment = pattern.BrickVectors[previousVectorIndex];
                pattern.BrickVectors[previousVectorIndex] = pattern.BrickVectors[i];
                pattern.BrickVectors[i] = previousSegment;
                previousVectorIndex--;
            }
        }
        return base.RequiresConstantRepaint();
    }
}