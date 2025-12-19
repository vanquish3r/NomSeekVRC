
using UnityEngine;
using UnityEditor;
using VRC.SDKBase;
using System.Text.RegularExpressions;

namespace Nomlas.NomSeekVRC.Editor
{
    [CustomEditor(typeof(VRCURLSetter))]
    public class VRCURLSetterEditor : UnityEditor.Editor
    {
        internal const string ConsentKey = "Nomlas.NomSeekVRC.AgreeApiUsage";
        private bool showDefaultInspector = false;
        private string worldID = string.Empty;
        private int poolSize = 10000;

        public override void OnInspectorGUI()
        {
            bool agreed = EditorPrefs.GetBool(ConsentKey, false);
            if (agreed)
            {
                DrawInspector();
            }
            else
            {
                DrawConsentSection();
            }
        }

        internal static bool IsAgreed()
        {
            return EditorPrefs.GetBool(ConsentKey, false);
        }

        private static void DrawConsentSection()
        {
            EditorGUILayout.LabelField("API 利用同意", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox("本アセットは、Lamp氏が提供する「VRChat YouTube Search API」を使用しています。\n本アセットおよび当該APIの利用または利用不能に起因して利用者に発生した損害（直接的・間接的・特別・偶発的・結果的損害を含むがこれらに限りません）について、製作者である「のむらす」は一切の責任を負わないものとします。\n以上に同意しますか？", MessageType.Warning);

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("同意しない"))
            {
                EditorPrefs.SetBool(ConsentKey, false);
            }
            if (GUILayout.Button("同意する"))
            {
                EditorPrefs.SetBool(ConsentKey, true);
            }
            EditorGUILayout.EndHorizontal();
        }

        [MenuItem("Tools/NomSeekVRC/同意状態のリセット")]
        private static void ResetApiConsent() => EditorPrefs.SetBool(ConsentKey, false);

        private void DrawInspector()
        {
            EditorGUILayout.LabelField("URL Generator", EditorStyles.boldLabel);

            worldID = EditorGUILayout.TextField("World ID", worldID);
            EditorGUILayout.HelpBox("英子文字・数字・アンダースコア・ハイフンのみ使用可能です", MessageType.None);
            poolSize = EditorGUILayout.IntField("Pool Size", poolSize);

            if (poolSize < 1) poolSize = 1;

            EditorGUI.BeginDisabledGroup(!IsValid(worldID));
            if (GUILayout.Button("Generate URLs")) GenerateUrls();
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.Space();

            showDefaultInspector = EditorGUILayout.Foldout(showDefaultInspector, "格納されているURL");
            if (showDefaultInspector)
            {
                EditorGUI.BeginDisabledGroup(true);
                DrawDefaultInspector();
                EditorGUI.EndDisabledGroup();
            }
        }

        private void GenerateUrls()
        {
            VRCURLSetter setter = (VRCURLSetter)target;

            string baseUrl = $"{VRCURLSetter.apiEndpoint}/vrcurl/{worldID}{poolSize}/";

            int arraySize = poolSize + 1;
            var urlArray = new VRCUrl[arraySize];
            for (int i = 0; i < arraySize; i++)
            {
                urlArray[i] = new VRCUrl($"{baseUrl}{i}");
            }

            setter.urls = urlArray;

            string needUrl = $"{VRCURLSetter.apiEndpoint}/search?pool={worldID}{poolSize}&thumbnails=true&icons=true&mode=latestontop&input=";
            setter.defaultVRCUrl = new VRCUrl(needUrl + "      ここに検索ワードを入力 →                                      ");
            setter.needUrl = needUrl;

            if (!Application.isPlaying)
            {
                EditorUtility.SetDirty(setter);
            }
        }

        public static bool IsValid(string worldId)
        {
            if (string.IsNullOrWhiteSpace(worldId)) return false;

            const string pattern = @"^[a-z0-9_-]+$";
            return Regex.IsMatch(worldId, pattern);
        }
    }
}