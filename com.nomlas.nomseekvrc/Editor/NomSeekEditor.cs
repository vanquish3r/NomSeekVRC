using UnityEditor;
using UnityEngine;
using VRC.SDKBase.Editor.BuildPipeline;

namespace Nomlas.NomSeekVRC.Editor
{
    [CustomEditor(typeof(NomSeek))]
    public class NomSeekEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var nomSeek = target as NomSeek;
            DrawDefaultInspector();

            if (!IsReady(nomSeek))
            {
                EditorGUILayout.Space();
                EditorGUILayout.HelpBox("VRCURLSetterの設定が完了していません！", MessageType.Warning);
            }
        }

        public static bool IsReady(NomSeek nomSeek)
        {
            return nomSeek.vrcurlSetter != null && nomSeek.vrcurlSetter.IsValid() && VRCURLSetterEditor.IsAgreed();
        }
    }

    public class VRCSDKBuildCallback : IVRCSDKBuildRequestedCallback
    {
        public int callbackOrder => -1;
        public bool OnBuildRequested(VRCSDKRequestedBuildType requestedBuildType)
        {
            NomSeek[] nomSeeks = Object.FindObjectsOfType<NomSeek>();
            
            foreach (var nomSeek in nomSeeks)
            {
                if (!NomSeekEditor.IsReady(nomSeek))
                {
                    EditorUtility.DisplayDialog("操作が完了していません", $"NomSeek For VRCのURL生成インスペクターで操作を完了してください。\n「OK」をクリックするとビルドを中止します。", "OK");
                    return false;
                }
            }
            return true;
        }
    }
}