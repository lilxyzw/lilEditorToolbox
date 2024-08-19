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

        [Header("Texture Import")]
        public bool turnOffCrunchCompression = true;
        public bool turnOnStreamingMipmaps = true;

        [Header("Model Import")]
        public bool turnOnReadable = true;
        public bool fixBlendshapes = true;

        [Header("Hierarchy")]
        public Color backgroundColor = new Color(0.5f,0.5f,0.5f,0.05f);
        public Color lineColor = new Color(0.5f,0.5f,0.5f,0.33f);
        public string[] hierarchyComponents = HierarchyExtention.names;

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
            while(iterator.NextVisible(false))
            {
                if(iterator.name != "hierarchyComponents")
                {
                    EditorGUILayout.PropertyField(iterator, true);
                }
                else
                {
                    var vals = Enumerable.Range(0, iterator.arraySize).Select(i => iterator.GetArrayElementAtIndex(i).stringValue).ToList();
                    foreach(var name in HierarchyExtention.names)
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
            }
            if(EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                EditorToolboxSettings.instance.Save();
                HierarchyExtention.Resolve();
                EditorApplication.RepaintHierarchyWindow();
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
