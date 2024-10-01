using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using UnityEditor;
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

        private static string folder;
        private static bool needToReplace = false;

        [InitializeOnLoadMethod]
        internal static void Init()
        {
            AssetDatabase.importPackageStarted -= OnImportPackageStarted;
            if(EditorToolboxSettings.instance.unitypackageToFolder) AssetDatabase.importPackageStarted += OnImportPackageStarted;
        }

        internal static void OnImportPackageStarted(string _)
        {
            if(DragAndDrop.paths.Length == 0) return;
            folder = ProjectWindowPath();
            needToReplace = !string.IsNullOrEmpty(folder) && folder.StartsWith("Assets/");
            runtime.CoroutineHandler.StartStaticCoroutine(ClosePackageImportWindow());
        }

        private static string ProjectWindowPath()
        {
            var folders = FI_m_LastFolders.GetValue(FI_s_LastInteractedProjectBrowser.GetValue(null)) as string[];
            return folders.Length == 1 ? folders[0] : "Assets/";
        }

        private static IEnumerator ClosePackageImportWindow()
        {
            while(!(bool)MI_HasOpenInstancesPackageImport.Invoke(null,null)) yield return null;
            var window = EditorWindow.GetWindow(T_PackageImport);
            var m_ImportPackageItems = FI_m_ImportPackageItems.GetValue(window) as object[];
            var items = m_ImportPackageItems.Select(o => new ImportPackageItemWrap(o)).ToArray();

            // パスの置換
            if(needToReplace)
            {
                foreach(var item in items)
                {
                    if(!string.IsNullOrEmpty(item.existingAssetPath) || !item.destinationAssetPath.StartsWith("Assets/")) continue;
                    item.destinationAssetPath = folder + item.destinationAssetPath.Substring("Assets".Length);
                }
                FI_m_PackageName.SetValue(window, $"{FI_m_PackageName.GetValue(window) as string} to \"{folder}\"");
            }

            // 既存アセットの場所を表示
            var ShowTreeGUI = (bool)MI_ShowTreeGUI.Invoke(window, new object[]{m_ImportPackageItems});
            if(!ShowTreeGUI)
            {
                var button = new Button(){text = "Show Import Window"};
                button.style.marginTop = 36;
                button.style.width = 150;
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
