using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Nomlas.NomSeekVRC.Editor
{
    [CustomEditor(typeof(ColorChanger))]
    public class ColorChangerEditor : UnityEditor.Editor
    {
        private SerializedProperty presetProp;
        private SerializedProperty imagesProp;
        private SerializedProperty textsProp;
        private SerializedProperty iconsProp;
        private SerializedProperty imagesColorProp;
        private SerializedProperty textColorProp;

        private int selectedPresetIndex = -1;
        private string[] presetNames;

        private void OnEnable()
        {
            presetProp = serializedObject.FindProperty("preset");
            imagesProp = serializedObject.FindProperty("images");
            textsProp = serializedObject.FindProperty("texts");
            iconsProp = serializedObject.FindProperty("icons");
            imagesColorProp = serializedObject.FindProperty("imagesColor");
            textColorProp = serializedObject.FindProperty("textColor");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.LabelField("Color Preset", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(presetProp);

            var presetObj = presetProp.objectReferenceValue as ColorPresetList;
            if (presetObj != null && presetObj.presets != null && presetObj.presets.Count > 0)
            {
                if (presetNames == null || presetNames.Length != presetObj.presets.Count)
                {
                    presetNames = new string[presetObj.presets.Count];
                    for (int i = 0; i < presetObj.presets.Count; i++)
                    {
                        presetNames[i] = presetObj.presets[i].presetName;
                    }
                }

                int newIndex = EditorGUILayout.Popup("Preset", selectedPresetIndex, presetNames);
                if (newIndex != selectedPresetIndex)
                {
                    selectedPresetIndex = newIndex;
                    if (selectedPresetIndex >= 0 && selectedPresetIndex < presetObj.presets.Count)
                    {
                        Undo.RecordObject(target, "Apply Color Preset");
                        var preset = presetObj.presets[selectedPresetIndex];
                        imagesColorProp.colorValue = preset.imageColor;
                        textColorProp.colorValue = preset.textColor;
                        serializedObject.ApplyModifiedProperties();

                        // Prefab対応
                        PrefabUtility.RecordPrefabInstancePropertyModifications(target);
                        EditorUtility.SetDirty(target);
                        if (!Application.isPlaying)
                        {
                            EditorSceneManager.MarkSceneDirty(
                                UnityEngine.SceneManagement.SceneManager.GetActiveScene());
                        }
                    }
                }
            }
            else
            {
                EditorGUILayout.HelpBox("ColorPresetListにプリセットがありません。", MessageType.Info);
            }

            EditorGUILayout.Space(10);

            EditorGUILayout.LabelField("Target Objects", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(imagesProp, new GUIContent("Images"), true);
            EditorGUILayout.PropertyField(textsProp, new GUIContent("Texts"), true);
            EditorGUILayout.PropertyField(iconsProp, new GUIContent("Icons"), true);

            EditorGUILayout.Space(10);

            EditorGUILayout.LabelField("Colors", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(imagesColorProp, new GUIContent("Images Color"));
            EditorGUILayout.PropertyField(textColorProp, new GUIContent("Text/Icon Color"));

            EditorGUILayout.Space(10);

            if (GUILayout.Button("Apply Color To Objects"))
            {
                Undo.RecordObject(target, "Apply Color To Objects");
                (target as ColorChanger).ApplyColor();

                // Prefab対応
                PrefabUtility.RecordPrefabInstancePropertyModifications(target);
                EditorUtility.SetDirty(target);
                if (!Application.isPlaying)
                {
                    EditorSceneManager.MarkSceneDirty(
                        UnityEngine.SceneManagement.SceneManager.GetActiveScene());
                }
            }

            EditorGUILayout.HelpBox("色の変更が保存されない時はPrefab展開してください。", MessageType.Info);

            serializedObject.ApplyModifiedProperties();
        }
    }
}