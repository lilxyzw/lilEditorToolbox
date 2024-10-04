using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace jp.lilxyzw.editortoolbox
{
    internal class UnitypackageImporter : ScriptableSingleton<UnitypackageImporter>
    {
        public List<string> importedAssets = new();
        private static List<string> importingAssets = new();


        [InitializeOnLoadMethod]
        internal static void Init()
        {
            AssetDatabase.importPackageStarted -= OnImportPackageStarted;
            AssetDatabase.importPackageStarted += OnImportPackageStarted;
            AssetDatabase.importPackageCompleted -= OnImportPackageCompleted;
            AssetDatabase.importPackageCompleted += OnImportPackageCompleted;
        }

        internal static void OnImportPackageCompleted(string _)
        {
            instance.importedAssets = importingAssets;
        }

        internal static void OnImportPackageStarted(string _)
        {
            if(DragAndDrop.paths.Length == 0) return;
            var folder = ProjectWindowPath();
            runtime.CoroutineHandler.StartStaticCoroutine(ClosePackageImportWindow(folder));
        }

        private static string ProjectWindowPath()
        {
            var folders = ProjectBrowserWrap.s_LastInteractedProjectBrowser.m_LastFolders;
            return folders.Length == 1 ? folders[0] : "Assets/";
        }

        private static IEnumerator ClosePackageImportWindow(string folder)
        {
            while(!PackageImportWrap.HasOpenInstances()) yield return null;
            var window = new PackageImportWrap(EditorWindow.GetWindow(PackageImportWrap.type));
            var m_ImportPackageItems = window.m_ImportPackageItems;
            var items = m_ImportPackageItems.Select(o => new ImportPackageItemWrap(o)).ToArray();

            importingAssets = items.Where(i => i.assetChanged).Select(i => i.destinationAssetPath).ToList();

            // Packages配下の上書き防止
            if(EditorToolboxSettings.instance.cancelUnitypackageOverwriteInPackages)
            {
                foreach(var item in items)
                {
                    var dest = item.destinationAssetPath;
                    if(!dest.StartsWith("Packages/") || !Directory.Exists(dest[..(dest.IndexOf('/', "Packages/".Length) + 1)])) continue;
                    item.assetChanged = false;
                }
            }

            // 既存アセットの場所を表示
            if(window.ShowTreeGUI(m_ImportPackageItems))
            {
                if(string.IsNullOrEmpty(folder) || !folder.StartsWith("Assets/")) yield break;
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
                window.w.rootVisualElement.Add(root);
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
                    window.w.rootVisualElement.Remove(button);
                };
                window.w.rootVisualElement.Add(button);
            }
        }
    }
}
