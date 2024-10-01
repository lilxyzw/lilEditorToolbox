using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace jp.lilxyzw.editortoolbox
{
    [FilePath("editortoolbox.asset", FilePathAttribute.Location.PreferencesFolder)]
    internal class EditorToolboxSettings : ScriptableSingleton<EditorToolboxSettings>
    {
        [Header("Language")]
        public string language = CultureInfo.CurrentCulture.Name;

        [L10nHeader("Asset Import")]
        [ToggleLeft] public bool dragAndDropOverwrite = true;
        [ToggleLeft] public bool unitypackageToFolder = true;

        [L10nHeader("Texture Import")]
        [ToggleLeft] public bool turnOffCrunchCompression = true;
        [ToggleLeft] public bool turnOnStreamingMipmaps = true;
        [ToggleLeft] public bool changeToKaiserMipmaps = true;

        [L10nHeader("Model Import")]
        [ToggleLeft] public bool turnOnReadable = true;
        [ToggleLeft] public bool fixBlendshapes = true;
        [ToggleLeft] public bool removeJaw = true;

        [L10nHeader("Hierarchy Extension")]
        public string[] hierarchyComponents = HierarchyExtension.names;

        [L10nHeader("Project Extension")]
        public string[] projectComponents = ProjectExtension.names;

        [L10nHeader("Toolbar Extension")]
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
                    EditorGUILayout.LabelField(L10n.G("Hierarchy Extension"), EditorStyles.boldLabel);
                    StringListAsToggle(HierarchyExtension.names);
                }
                else if(iterator.name == "projectComponents")
                {
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField(L10n.G("Project Extension"), EditorStyles.boldLabel);
                    StringListAsToggle(ProjectExtension.names);
                }
                else if(iterator.name == "toolbarComponents")
                {
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField(L10n.G("Toolbar Extension"), EditorStyles.boldLabel);
                    StringListAsToggle(ToolbarExtension.names);
                }
                else if(iterator.name == "language")
                {
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Language", EditorStyles.boldLabel);

                    var langs = L10n.GetLanguages();
                    var names = L10n.GetLanguageNames();
                    EditorGUI.BeginChangeCheck();
                    var ind = EditorGUILayout.Popup("Language", Array.IndexOf(langs, iterator.stringValue), names);
                    if(EditorGUI.EndChangeCheck()) iterator.stringValue = langs[ind];
                }
                else
                {
                    EditorGUILayout.PropertyField(iterator, L10n.G(iterator.displayName), true);
                }
            }

            if(EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                L10n.Load();
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
