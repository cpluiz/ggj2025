using UnityEngine;
using UnityEditor;
using CTBW.Settings;

[CustomEditor(typeof(ScenePicker), true)]
public class ScenePickerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var picker = target as ScenePicker;
        var oldScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(picker.scenePath);

        serializedObject.Update();

        EditorGUI.BeginChangeCheck();
        var newScene = EditorGUILayout.ObjectField("scene", oldScene, typeof(SceneAsset), false) as SceneAsset;

        if (EditorGUI.EndChangeCheck())
        {
            var newPath = AssetDatabase.GetAssetPath(newScene);
            var scenePathProperty = serializedObject.FindProperty("scenePath");
            scenePathProperty.stringValue = newPath;
        }
        serializedObject.ApplyModifiedProperties();
    }
}