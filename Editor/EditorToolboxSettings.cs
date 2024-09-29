using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace jp.lilxyzw.editortoolbox
{
    [FilePath("editortoolbox.asset", FilePathAttribute.Location.PreferencesFolder)]
    internal class EditorToolboxSettings : ScriptableSingleton<EditorToolboxSettings>
    {
        [Header("Asset Import")]
        public bool dragAndDropOverwrite = true;
        public bool unitypackageToFolder = true;

        [Header("Texture Import")]
        public bool turnOffCrunchCompression = true;
        public bool turnOnStreamingMipmaps = true;
        public bool changeToKaiserMipmaps = true;

        [Header("Model Import")]
        public bool turnOnReadable = true;
        public bool fixBlendshapes = true;
        public bool removeJaw = true;

        [Header("Hierarchy")]
        public string[] hierarchyComponents = HierarchyExtension.names;

        [Header("Project")]
        public string[] projectComponents = ProjectExtension.names;

        [Header("Toolbar")]
        public string[] toolbarComponents = ToolbarExtension.names;

        internal readonly Color backgroundColor = new Color(0.5f,0.5f,0.5f,0.05f);
        internal readonly Color lineColor = new Color(0.5f,0.5f,0.5f,0.33f);

        internal void Save() => Save(true);
    }

    [CustomEditor(typeof(EditorToolboxSettings))]
    internal class EditorToolboxSettingsEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            serializedObject.UpdateIfRequiredOrScript();
            SerializedProperty iterator = serializedObject.GetIterator();
            iterator.NextVisible(true); // m_Script
            void StringListAsToggle(string[] names)
            {
                var vals = Enumerable.Range(0, iterator.arraySize).Select(i => iterator.GetArrayElementAtIndex(i).stringValue).ToList();
                foreach(var name in names)
                {
                    var contains = vals.Contains(name);
                    var toggle = EditorGUILayout.ToggleLeft(name, contains);
                    if(contains != toggle)
                    {
                        if(toggle)
                        {
                            iterator.InsertArrayElementAtIndex(iterator.arraySize);
                            iterator.GetArrayElementAtIndex(iterator.arraySize-1).stringValue = name;
                        }
                        else
                        {
                            iterator.DeleteArrayElementAtIndex(vals.IndexOf(name));
                        }
                    }
                }
            }
            while(iterator.NextVisible(false))
            {
                if(iterator.name == "hierarchyComponents")
                {
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Hierarchy", EditorStyles.boldLabel);
                    StringListAsToggle(HierarchyExtension.names);
                }
                else if(iterator.name == "projectComponents")
                {
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Project", EditorStyles.boldLabel);
                    StringListAsToggle(ProjectExtension.names);
                }
                else if(iterator.name == "toolbarComponents")
                {
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Toolbar", EditorStyles.boldLabel);
                    StringListAsToggle(ToolbarExtension.names);
                }
                else
                {
                    EditorGUILayout.PropertyField(iterator, true);
                }
            }
            if(EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                UnitypackageImporter.Init();
                EditorToolboxSettings.instance.Save();
                HierarchyExtension.Resolve();
                ProjectExtension.Resolve();
                ToolbarExtension.Resolve();
                EditorApplication.RepaintHierarchyWindow();
                EditorApplication.RepaintProjectWindow();
            }
        }
    }

    internal class EditorToolboxSettingsProvider : EasySettingProvider
    {
        public EditorToolboxSettingsProvider(string path, SettingsScope scopes, IEnumerable<string> keywords) : base(path, scopes, keywords){}
        public override ScriptableObject SO => EditorToolboxSettings.instance;
        [SettingsProvider] public static SettingsProvider Create() => Create("lilEditorToolbox");
    }
}
