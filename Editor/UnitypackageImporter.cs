using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using UnityEditor;

namespace jp.lilxyzw.editortoolbox
{
    internal class UnitypackageImporter
    {
        [InitializeOnLoadMethod]
        internal static void Init()
        {
            AssetDatabase.importPackageStarted -= OnImportPackageStarted;
            if(EditorToolboxSettings.instance.unitypackageToFolder) AssetDatabase.importPackageStarted += OnImportPackageStarted;
        }

        private static string folder;

        internal static void OnImportPackageStarted(string _)
        {
            if(DragAndDrop.paths.Length == 0) return;
            folder = ProjectWindowPath();
            if(string.IsNullOrEmpty(folder) || !folder.StartsWith("Assets/")) return;
            CoroutineHandler.StartStaticCoroutine(ClosePackageImportWindow());
        }

        private static string ProjectWindowPath()
        {
            var type = typeof(Editor).Assembly.GetType("UnityEditor.ProjectBrowser");
            var window = type.GetField("s_LastInteractedProjectBrowser", BindingFlags.Static | BindingFlags.Public).GetValue(null);
            var folders = type.GetField("m_LastFolders", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(window) as string[];
            return folders.Length == 1 ? folders[0] : "Assets/";
        }

        private static IEnumerator ClosePackageImportWindow()
        {
            var type = typeof(Editor).Assembly.GetType("UnityEditor.PackageImport");
            var method = typeof(EditorWindow).GetMethod("HasOpenInstances", BindingFlags.Static | BindingFlags.Public);
            if(method != null)
            {
                var genmethod = method.MakeGenericMethod(type);
                while(!(bool)genmethod.Invoke(null,null))
                {
                    yield return null;
                }
                var window = EditorWindow.GetWindow(type);
                var items = (type.GetField("m_ImportPackageItems", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(window) as object[]).Select(o => new ImportPackageItemWrap(o)).ToArray();
                foreach(var item in items)
                {
                    if(!string.IsNullOrEmpty(item.existingAssetPath) || !item.destinationAssetPath.StartsWith("Assets/")) continue;
                    item.destinationAssetPath = folder + item.destinationAssetPath.Substring("Assets".Length);
                }
                var FI_m_PackageName = type.GetField("m_PackageName", BindingFlags.NonPublic | BindingFlags.Instance);
                FI_m_PackageName.SetValue(window, $"{FI_m_PackageName.GetValue(window) as string} to \"{folder}\"");
            }
        }
    }

    internal class ImportPackageItemWrap
    {
        private static readonly Type TYPE = typeof(Editor).Assembly.GetType("UnityEditor.ImportPackageItem");
        private static readonly FieldInfo FI_existingAssetPath = TYPE.GetField("existingAssetPath", BindingFlags.Public | BindingFlags.Instance);
        private static readonly FieldInfo FI_destinationAssetPath = TYPE.GetField("destinationAssetPath", BindingFlags.Public | BindingFlags.Instance);
        private object instance;
        public string existingAssetPath => FI_existingAssetPath.GetValue(instance) as string;
        public string destinationAssetPath
        {
            get => FI_destinationAssetPath.GetValue(instance) as string;
            set => FI_destinationAssetPath.SetValue(instance, value);
        }

        public ImportPackageItemWrap(object i) => instance = i;
    }
}
