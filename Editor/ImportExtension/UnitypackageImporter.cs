using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace jp.lilxyzw.editortoolbox
{
    internal class UnitypackageImporter
    {
        private static readonly MethodInfo MI_HasOpenInstances = typeof(EditorWindow).GetMethod("HasOpenInstances", BindingFlags.Static | BindingFlags.Public);

        private static readonly Type T_ProjectBrowser = typeof(Editor).Assembly.GetType("UnityEditor.ProjectBrowser");
        private static readonly FieldInfo FI_s_LastInteractedProjectBrowser = T_ProjectBrowser.GetField("s_LastInteractedProjectBrowser", BindingFlags.Static | BindingFlags.Public);
        private static readonly FieldInfo FI_m_LastFolders = T_ProjectBrowser.GetField("m_LastFolders", BindingFlags.NonPublic | BindingFlags.Instance);

        private static readonly Type T_PackageImport = typeof(Editor).Assembly.GetType("UnityEditor.PackageImport");
        private static readonly MethodInfo MI_HasOpenInstancesPackageImport = MI_HasOpenInstances.MakeGenericMethod(T_PackageImport);
        private static readonly FieldInfo FI_m_ImportPackageItems = T_PackageImport.GetField("m_ImportPackageItems", BindingFlags.NonPublic | BindingFlags.Instance);
        private static readonly FieldInfo FI_m_PackageName = T_PackageImport.GetField("m_PackageName", BindingFlags.NonPublic | BindingFlags.Instance);
        private static readonly MethodInfo MI_ShowTreeGUI = T_PackageImport.GetMethod("ShowTreeGUI", BindingFlags.NonPublic | BindingFlags.Instance);

        [InitializeOnLoadMethod]
        internal static void Init()
        {
            AssetDatabase.importPackageStarted -= OnImportPackageStarted;
            AssetDatabase.importPackageStarted += OnImportPackageStarted;
        }

        internal static void OnImportPackageStarted(string _)
        {
            if(DragAndDrop.paths.Length == 0) return;
            var folder = ProjectWindowPath();
            var needToReplace = !string.IsNullOrEmpty(folder) && folder.StartsWith("Assets/");
            runtime.CoroutineHandler.StartStaticCoroutine(ClosePackageImportWindow(needToReplace, folder));
        }

        private static string ProjectWindowPath()
        {
            var folders = FI_m_LastFolders.GetValue(FI_s_LastInteractedProjectBrowser.GetValue(null)) as string[];
            return folders.Length == 1 ? folders[0] : "Assets/";
        }

        private static IEnumerator ClosePackageImportWindow(bool needToReplace, string folder)
        {
            while(!(bool)MI_HasOpenInstancesPackageImport.Invoke(null,null)) yield return null;
            var window = EditorWindow.GetWindow(T_PackageImport);
            var m_ImportPackageItems = FI_m_ImportPackageItems.GetValue(window) as object[];
            var items = m_ImportPackageItems.Select(o => new ImportPackageItemWrap(o)).ToArray();

            // 既存アセットの場所を表示
            var ShowTreeGUI = (bool)MI_ShowTreeGUI.Invoke(window, new object[]{m_ImportPackageItems});
            if(ShowTreeGUI)
            {
                if(!needToReplace) yield break;
                var origins = new Dictionary<object, string>();
                var root = new VisualElement();
                root.style.marginLeft = 6;
                root.style.alignItems = Align.Center;
                root.style.flexDirection = FlexDirection.Row;
                root.Add(new Label(L10n.L("Import directory")));

                var button = new Button(){text = "Assets/"};
                EditorStyles.label.CalcMinMaxWidth(new GUIContent(button.text), out float minWidth, out float maxWidth);
                button.style.width = maxWidth + 16;
                button.clicked += () => {
                    if(origins.Count == 0)
                    {
                        foreach(var item in items)
                        {
                            if(!string.IsNullOrEmpty(item.existingAssetPath) || !item.destinationAssetPath.StartsWith("Assets/")) continue;
                            origins[item] = item.destinationAssetPath;
                            item.destinationAssetPath = folder + item.destinationAssetPath.Substring("Assets".Length);
                        }
                        button.text = folder+"/";
                        EditorStyles.label.CalcMinMaxWidth(new GUIContent(button.text), out float minWidth, out float maxWidth);
                        button.style.width = maxWidth + 16;
                    }
                    else
                    {
                        foreach(var item in items)
                            if(origins.TryGetValue(item, out var path)) item.destinationAssetPath = path;
                        origins.Clear();
                        button.text = "Assets/";
                        EditorStyles.label.CalcMinMaxWidth(new GUIContent(button.text), out float minWidth, out float maxWidth);
                        button.style.width = maxWidth + 16;
                    }
                };

                root.Add(button);
                window.rootVisualElement.Add(root);
            }
            else
            {
                var button = new Button(){text = L10n.L("Show Import Window")};
                EditorStyles.label.CalcMinMaxWidth(new GUIContent(button.text), out float minWidth, out float maxWidth);
                button.style.marginTop = 36;
                button.style.width = maxWidth + 32;
                button.clicked += () => {
                    foreach(var i in items)
                        if(!i.isFolder) i.assetChanged = true;
                    window.rootVisualElement.Remove(button);
                };
                window.rootVisualElement.Add(button);
            }
        }
    }

    internal class ImportPackageItemWrap
    {
        private static readonly Type TYPE = typeof(Editor).Assembly.GetType("UnityEditor.ImportPackageItem");
        private static readonly FieldInfo FI_existingAssetPath = TYPE.GetField("existingAssetPath", BindingFlags.Public | BindingFlags.Instance);
        private static readonly FieldInfo FI_destinationAssetPath = TYPE.GetField("destinationAssetPath", BindingFlags.Public | BindingFlags.Instance);
        private static readonly FieldInfo FI_isFolder = TYPE.GetField("isFolder", BindingFlags.Public | BindingFlags.Instance);
        private static readonly FieldInfo FI_assetChanged = TYPE.GetField("assetChanged", BindingFlags.Public | BindingFlags.Instance);
        private object instance;
        public string existingAssetPath => FI_existingAssetPath.GetValue(instance) as string;
        public string destinationAssetPath
        {
            get => FI_destinationAssetPath.GetValue(instance) as string;
            set => FI_destinationAssetPath.SetValue(instance, value);
        }
        public bool isFolder => (bool)FI_isFolder.GetValue(instance);
        public bool assetChanged
        {
            get => (bool)FI_assetChanged.GetValue(instance);
            set => FI_assetChanged.SetValue(instance, value);
        }

        public ImportPackageItemWrap(object i) => instance = i;
    }
}
