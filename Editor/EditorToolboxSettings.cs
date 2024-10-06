using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace jp.lilxyzw.editortoolbox
{
    [Docs(
        "Settings",
        "Settings related to lilEditorToolbox. You can open it from `Edit/Preference/lilEditorToolbox` on the menu bar, where you can change the language and enable various functions."
    )]
    [FilePath("jp.lilxyzw/editortoolbox.asset", FilePathAttribute.Location.PreferencesFolder)]
    internal class EditorToolboxSettings : ScriptableSingleton<EditorToolboxSettings>
    {
        [Header("Language")]
        [Tooltip("The language setting for lilEditorToolbox. The language file exists in `jp.lilxyzw.editortoolbox/Editor/Localization`, and you can support other languages by creating a language file.")]
        public string language = CultureInfo.CurrentCulture.Name;

        [L10nHeader("Asset Import")]
        [Tooltip("When importing assets via D&D, if there is a file with the same name at the same level, it will be overwritten by the import.")]
        [ToggleLeft] public bool dragAndDropOverwrite = false;
        [Tooltip("Prevents unitypackage from overwriting assets under Packages.")]
        [ToggleLeft] public bool cancelUnitypackageOverwriteInPackages = false;

        [L10nHeader("Texture Import")]
        [Tooltip("Automatically turn off Crunch Compression when importing textures to speed up imports.")]
        [ToggleLeft] public bool turnOffCrunchCompression = false;
        [Tooltip("Automatically turn on `Streaming Mipmaps` when importing textures.")]
        [ToggleLeft] public bool turnOnStreamingMipmaps = false;
        [Tooltip("Automatically change to `Kaiser` when importing texture.")]
        [ToggleLeft] public bool changeToKaiserMipmaps = false;

        [L10nHeader("Model Import")]
        [Tooltip("Automatically turn on `Readable` when importing a model.")]
        [ToggleLeft] public bool turnOnReadable = false;
        [Tooltip("Turn off `Legacy Blend Shape Normals` when importing a model to turn off automatic recalculation of BlendShape normals.")]
        [ToggleLeft] public bool fixBlendshapes = false;
        [Tooltip("When importing a model, if a bone that does not contain `jaw` (case insensitive) in the bone name is assigned to the Humanoid Jaw, it will be automatically unassigned.")]
        [ToggleLeft] public bool removeJaw = false;

        [L10nHeader("Hierarchy Extension", "You can display objects, components, tags, layers, etc. on the Hierarchy. You can also add your own extensions by implementing `IHierarchyExtensionConponent`. Please refer to the scripts under `Editor/HierarchyExtension/Components` for how to write them.")]
        [Tooltip("The width of the margin to avoid interfering with other Hierarchy extensions.")]
        public int hierarchySpacerWidth = 0;
        [Tooltip("This is the timing to insert margins so as not to interfere with other Hierarchy extensions.")]
        public int hierarchySpacerPriority = 0;
        [DocsGetStrings(typeof(HierarchyExtension), "GetNameAndTooltips")]
        public string[] hierarchyComponents = new string[]{};

        [L10nHeader("Project Extension", "You can display extensions and prefab information on the project. You can also add your own extensions by implementing `IProjectExtensionConponent`. Please refer to the script under `Editor/ProjectExtension/Components` for how to write it.")]
        [DocsGetStrings(typeof(ProjectExtension), "GetNameAndTooltips")]
        public string[] projectComponents = new string[]{};

        [L10nHeader("Toolbar Extension", "You can display an assembly lock button or an extension inspector tab on the Toolbar. You can also add your own extension by implementing `IToolbarExtensionComponent`. Please refer to the script under `Editor/ToolbarExtension/Components` for how to write it.")]
        [DocsGetStrings(typeof(ToolbarExtension), "GetNameAndTooltips")]
        public string[] toolbarComponents = new string[]{};

        [L10nHeader("Menu Directory Replaces", "You can customize the menu bar by changing or deleting the menu hierarchy. You can also edit multiple menus at once by specifying `Tools/*`.")]
        [L10nHelpBox("Some menu items may not be compatible with replacement.", MessageType.Warning)]
        [Tooltip("When this check box is selected, changing and deleting the menu hierarchy is enabled.")] [ToggleLeft] public bool enableMenuDirectoryReplaces = false;
        [Tooltip("Add the menu to be changed here. If To is empty, the menu will be deleted, if it is not empty, it will be moved to that hierarchy.")] public MenuReplace[] menuDirectoryReplaces = new MenuReplace[]{};

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
            if(L10n.ButtonLimited("Open preference folder"))
            {
                System.Diagnostics.Process.Start(UnityEditorInternal.InternalEditorUtility.unityPreferencesFolder + "/jp.lilxyzw");
            }

            EditorGUI.BeginChangeCheck();
            serializedObject.UpdateIfRequiredOrScript();
            SerializedProperty iterator = serializedObject.GetIterator();
            iterator.NextVisible(true); // m_Script
            void StringListAsToggle((string[] key, string fullname)[] names)
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                var vals = Enumerable.Range(0, iterator.arraySize).Select(i => iterator.GetArrayElementAtIndex(i).stringValue).ToList();
                foreach(var name in names)
                {
                    var contains = vals.Contains(name.fullname);
                    string label = L10n.L(name.key[0]);
                    if(!name.fullname.StartsWith("jp.lilxyzw.editortoolbox")) label = $"{label} ({name.fullname})";
                    var toggle = EditorGUILayout.ToggleLeft(L10n.G(label, name.key[1]), contains);
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
                EditorGUILayout.EndVertical();
            }

            while(iterator.NextVisible(false))
            {
                if(iterator.name == "hierarchyComponents")
                {
                    EditorGUILayout.Space();
                    StringListAsToggle(HierarchyExtension.names);
                }
                else if(iterator.name == "projectComponents")
                {
                    EditorGUILayout.Space();
                    L10n.LabelField("Project Extension", EditorStyles.boldLabel);
                    StringListAsToggle(ProjectExtension.names);
                }
                else if(iterator.name == "toolbarComponents")
                {
                    EditorGUILayout.Space();
                    L10n.LabelField("Toolbar Extension", EditorStyles.boldLabel);
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
                    L10n.PropertyField(iterator);
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
