using UnityEngine;

public static class GUIStyleUtility {

    public static GUIStyle boldFontStyle = new GUIStyle() { fontStyle = FontStyle.Bold };
    public static GUIStyle foldoutPadding = new GUIStyle() { padding = new RectOffset(23, 0, 0, 0) };
    public static GUIStyle rightPadding1 = new GUIStyle() { padding = new RectOffset(15, 0, 0, 0) };
    public static GUIStyle rightPadding2 = new GUIStyle() { padding = new RectOffset(30, 0, 0, 0) };
    public static GUIStyle leftToggleLabelStyle = new GUIStyle(boldFontStyle) { padding = new RectOffset(3, 0, 1, 0) };
}