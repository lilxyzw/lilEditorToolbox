using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace jp.lilxyzw.editortoolbox
{
    internal class UnitypackageHilighter : IProjectExtensionComponent
    {
        public int Priority => -1400;

        private static HashSet<string> guids = new();

        public void OnGUI(ref Rect currentRect, string guid, string path, string name, string extension, Rect fullRect)
        {
            if(guids.Contains(guid))
                EditorGUI.DrawRect(fullRect, EditorToolboxSettings.instance.backgroundHilightColor);
        }

        private class UnitypackageHilighterData : ScriptableSingleton<UnitypackageHilighterData>
        {
            public string target = "None";
        }

        [InitializeOnLoadMethod]
        private static void Init()
        {
            AssetDatabase.importPackageCompleted -= UpdateGUIDs;
            AssetDatabase.importPackageCompleted += UpdateGUIDs;
            EditorApplication.update -= SelectorButton.AddButton;
            EditorApplication.update += SelectorButton.AddButton;
            EditorToolboxSettingsEditor.update -= SelectorButton.UpdateButton;
            EditorToolboxSettingsEditor.update += SelectorButton.UpdateButton;
            UpdateGUIDs(UnitypackageHilighterData.instance.target);
        }

        private static void UpdateGUIDs(string name)
        {
            if(name != "None" && !name.EndsWith(".unitypackage")) name = $"{name}.unitypackage";
            var match = UnitypackageImporter.instance.importedAssets.LastOrDefault(i => i.name == name);
            if(match != null) guids = match.guids.ToHashSet();
            else guids.Clear();
            UnitypackageHilighterData.instance.target = name;
            SelectorButton.UpdateButton();
        }

        private class SelectorButton
        {
            private static IconPopup popup;
            private static List<string> names;
            private static List<string> Names => names != null && names.Count == UnitypackageImporter.instance.importedAssets.Count ? names : names = UnitypackageImporter.instance.importedAssets.Select(i => i.name).ToList();
            private static HashSet<EditorWindow> modifiedWindows = new();
            private static Texture tex = EditorGUIUtility.IconContent("LightingSettings Icon").image;

            internal static void AddButton()
            {
                var windows = ProjectBrowserWrap.GetAllProjectBrowsers();
                foreach(var window in windows) AddButton(window.w);
                EditorApplication.update -= AddButton;
            }

            private static void AddButton(EditorWindow window)
            {
                if(modifiedWindows.Add(window))
                {
                    if(popup == null) InitButton();
                    window.rootVisualElement.Add(popup);
                }
            }

            private static void InitButton()
            {
                popup = new(tex, Names.ToList(), UnitypackageHilighterData.instance.target);
                popup.style.marginLeft = 36;
                popup.style.height = 20;
                popup.style.width = 24;
                popup.valueChanged += UpdateGUIDs;
                popup.rightClicked += UnitypackageLogEditor.Init;
                UpdateButton();
            }

            internal static void UpdateButton()
            {
                if(popup == null)
                {
                    InitButton();
                    return;
                }
                popup.choices = Names;
                popup.value = UnitypackageHilighterData.instance.target;
                popup.visible = EditorToolboxSettings.instance.projectComponents.Any(c => c == typeof(UnitypackageHilighter).FullName);
            }

            private class UnitypackageLogEditor : EditorWindow
            {
                internal static void Init() => GetWindow(typeof(UnitypackageLogEditor)).Show();
                internal static List<string> namesLocal;

                void OnGUI()
                {
                    if(namesLocal == null || namesLocal.Count != (Names.Count-1))
                        namesLocal = Names.Where(n => n != "None").ToList();

                    int i = 1;
                    foreach(var n in namesLocal)
                    {
                        var rect = EditorGUILayout.GetControlRect();
                        var xMax = rect.xMax;
                        var buttonWidth = L10n.GetTextWidth("Remove") + 10;
                        rect.width -= buttonWidth + 4;
                        EditorGUI.LabelField(rect, n);
                        rect.xMin = rect.xMax;
                        rect.xMax = xMax;
                        EditorGUI.BeginChangeCheck();
                        L10n.Button(rect, "Remove");
                        if(EditorGUI.EndChangeCheck())
                        {
                            UnitypackageImporter.instance.importedAssets.RemoveAt(i);
                            UnitypackageImporter.instance.Save();
                            UpdateButton();
                        }
                        i++;
                    }
                }
            }
        }
    }
}
