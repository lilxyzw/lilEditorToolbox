using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace jp.lilxyzw.editortoolbox
{
    [FilePath("jp.lilxyzw/editortoolbox.asset", FilePathAttribute.Location.PreferencesFolder)]
    internal class EditorToolboxSettings : ScriptableSingleton<EditorToolboxSettings>
    {
        [Header("Language")]
        public string language = CultureInfo.CurrentCulture.Name;

        [L10nHeader("Asset Import")]
        [ToggleLeft] public bool dragAndDropOverwrite = false;
        [ToggleLeft] public bool cancelUnitypackageOverwriteInPackages = false;

        [L10nHeader("Texture Import")]
        [ToggleLeft] public bool turnOffCrunchCompression = false;
        [ToggleLeft] public bool turnOnStreamingMipmaps = false;
        [ToggleLeft] public bool changeToKaiserMipmaps = false;

        [L10nHeader("Model Import")]
        [ToggleLeft] public bool turnOnReadable = false;
        [ToggleLeft] public bool fixBlendshapes = false;
        [ToggleLeft] public bool removeJaw = false;

        [L10nHeader("Hierarchy Extension")]
        public string[] hierarchyComponents = new string[]{};

        [L10nHeader("Project Extension")]
        public string[] projectComponents = new string[]{};

        [L10nHeader("Toolbar Extension")]
        public string[] toolbarComponents = new string[]{};

        [L10nHeader("Menu Directory Replaces")]
        [L10nHelpBox("Some menu items may not be compatible with replacement.", MessageType.Warning)]
        [ToggleLeft] public bool enableMenuDirectoryReplaces = false;
        public MenuReplace[] menuDirectoryReplaces = new MenuReplace[]{};

        internal readonly Color backgroundColor = new Color(0.5f,0.5f,0.5f,0.05f);
        internal readonly Color lineColor = new Color(0.5f,0.5f,0.5f,0.33f);
        internal readonly Color backgroundHilightColor = new Color(1.0f,0.95f,0.5f,0.2f);

        internal void Save() => Save(true);
    }

    [CustomEditor(typeof(EditorToolboxSettings))]
    public class EditorToolboxSettingsEditor : Editor
    {
        public static Action update;

        public override void OnInspectorGUI()
        {
            var openPreferenceFolder = L10n.G("Open preference folder");
            if(GUILayout.Button(openPreferenceFolder, GUILayout.MaxWidth(Common.GetTextWidth(openPreferenceFolder)+16)))
            {
                System.Diagnostics.Process.Start(UnityEditorInternal.InternalEditorUtility.unityPreferencesFolder + "/jp.lilxyzw");
            }

            EditorGUI.BeginChangeCheck();
            serializedObject.UpdateIfRequiredOrScript();
            SerializedProperty iterator = serializedObject.GetIterator();
            iterator.NextVisible(true); // m_Script
            void StringListAsToggle((string key, string fullname)[] names)
            {
                var vals = Enumerable.Range(0, iterator.arraySize).Select(i => iterator.GetArrayElementAtIndex(i).stringValue).ToList();
                foreach(var name in names)
                {
                    var contains = vals.Contains(name.fullname);
                    string label = L10n.L(name.key);
                    if(!name.fullname.StartsWith("jp.lilxyzw.editortoolbox")) label = $"{label} ({name.fullname})";
                    var toggle = EditorGUILayout.ToggleLeft(label, contains);
                    if(contains != toggle)
                    {
                        if(toggle)
                        {
                            iterator.InsertArrayElementAtIndex(iterator.arraySize);
                            iterator.GetArrayElementAtIndex(iterator.arraySize-1).stringValue = name.fullname;
                        }
                        else
                        {
                            iterator.DeleteArrayElementAtIndex(vals.IndexOf(name.fullname));
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
                EditorToolboxSettings.instance.Save();
                EditorApplication.RepaintHierarchyWindow();
                EditorApplication.RepaintProjectWindow();
                update.Invoke();
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
