using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

namespace jp.lilxyzw.editortoolbox
{
    internal static class Grimoire
    {
        internal static readonly string PATH_DATABASE = $"{UnityEditorInternal.InternalEditorUtility.unityPreferencesFolder}/jp.lilxyzw/guid_database.zip";

        internal static void Scan(SerializedObject serializedObject, List<GUIDLibrary> libs)
        {
            SerializedProperty iter = serializedObject.GetIterator();
            bool enterChildren = true;
            while(iter.Next(enterChildren))
            {
                enterChildren = true;
                switch(iter.propertyType)
                {
                    case SerializedPropertyType.ObjectReference:
                        if(iter.objectReferenceValue || iter.objectReferenceInstanceIDValue == 0) break;
                        var guid = GlobalObjectId.GetGlobalObjectIdSlow(iter.objectReferenceInstanceIDValue).assetGUID.ToString();
                        if(!string.IsNullOrEmpty(AssetDatabase.GUIDToAssetPath(guid))) break;
                        var lib = GUIDLibrary.SearchLib(PATH_DATABASE, GUIDLibrary.CompressGUID(guid));
                        if(lib) libs.Add(lib);
                        break;
                    case SerializedPropertyType.String:
                        enterChildren = false;
                        break;
                }
            }
        }

        internal static void OnGUI(List<GUIDLibrary> libs)
        {
            if(libs.Count == 0) return;

            GUI.enabled = true;
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Missing assets", EditorStyles.boldLabel);

            foreach(var lib in libs)
            {
                GUI.enabled = true;
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                if(!string.IsNullOrEmpty(lib.displayName)) EditorGUILayout.SelectableLabel(lib.displayName, GUILayout.Height(EditorGUIUtility.singleLineHeight));
                if(!string.IsNullOrEmpty(lib.name)) EditorGUILayout.SelectableLabel(lib.name, GUILayout.Height(EditorGUIUtility.singleLineHeight));

                if(!string.IsNullOrEmpty(lib.url))
                {
                    EditorGUILayout.Space();
                    EditorGUILayout.SelectableLabel(lib.url, GUILayout.Height(EditorGUIUtility.singleLineHeight));
                    if(GUILayout.Button("Open web page")) Application.OpenURL(lib.url);
                }

                if(!string.IsNullOrEmpty(lib.repo))
                {
                    EditorGUILayout.Space();
                    EditorGUILayout.SelectableLabel(lib.repo, GUILayout.Height(EditorGUIUtility.singleLineHeight));
                    if(GUILayout.Button("Add to VCC")) Application.OpenURL($"vcc://vpm/addRepo?url={lib.url}");
                }

                EditorGUILayout.EndVertical();
            }
        }
    }

    internal class GrimoireBase : Editor
    {
        private readonly List<GUIDLibrary> libs = new List<GUIDLibrary>();
        private bool isScanned = false;
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if(!File.Exists(Grimoire.PATH_DATABASE)) return;
            if(!isScanned)
            {
                Grimoire.Scan(serializedObject, libs);
                isScanned = true;
            }
            Grimoire.OnGUI(libs);
        }
    }

    [CustomEditor(typeof(Material))]
    internal class Grimoire4Material : MaterialEditor
    {
        private readonly List<GUIDLibrary> libs = new List<GUIDLibrary>();
        private bool isScanned = false;
        private Shader shader;
        public override void OnInspectorGUI()
        {
            shader = ((Material)target).shader;
            if(shader && shader.name != "Hidden/InternalErrorShader") base.OnInspectorGUI();
            if(!File.Exists(Grimoire.PATH_DATABASE)) return;
            if(!isScanned)
            {
                Grimoire.Scan(serializedObject, libs);
                isScanned = true;
            }
            Grimoire.OnGUI(libs);
        }
    }

    [CustomEditor(typeof(MonoBehaviour), true)]
    internal class Grimoire4MonoBehaviour : GrimoireBase { }
}
