using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace jp.lilxyzw.editortoolbox
{
    internal class L10n : ScriptableSingleton<L10n>
    {
        public LocalizationAsset localizationAsset;
        private static string[] languages;
        private static string[] languageNames;
        private static readonly Dictionary<string, GUIContent> guicontents = new();
        private static string localizationFolder => AssetDatabase.GUIDToAssetPath("3e480d0d691fbaf4c82d5ea65c66a497");

        internal static void Load()
        {
            guicontents.Clear();
            var path = localizationFolder + "/" + EditorToolboxSettings.instance.language + ".po";
            if(File.Exists(path)) instance.localizationAsset = AssetDatabase.LoadAssetAtPath<LocalizationAsset>(path);

            if(!instance.localizationAsset) instance.localizationAsset = new LocalizationAsset();
        }

        internal static string[] GetLanguages()
        {
            return languages ??= Directory.GetFiles(localizationFolder).Where(f => f.EndsWith(".po")).Select(f => Path.GetFileNameWithoutExtension(f)).ToArray();
        }

        internal static string[] GetLanguageNames()
        {
            return languageNames ??= languages.Select(l => {
                if(l == "zh-Hans") return "简体中文";
                if(l == "zh-Hant") return "繁體中文";
                return new CultureInfo(l).NativeName;
            }).ToArray();
        }

        internal static string L(string key)
        {
            if(!instance.localizationAsset) Load();
            return instance.localizationAsset.GetLocalizedString(key);
        }

        internal static GUIContent G(string key)
        {
            if(!instance.localizationAsset) Load();
            if(guicontents.TryGetValue(key, out var content)) return content;
            return guicontents[key] = new GUIContent(L(key));
        }
    }

    internal class L10nHeaderAttribute : PropertyAttribute
    {
        public readonly string key;
        public string header => L10n.L(key);
        public L10nHeaderAttribute(string key)
        {
            this.key = key;
        }
    }

    [CustomPropertyDrawer(typeof(L10nHeaderAttribute))]
    internal class L10nHeaderDrawer : DecoratorDrawer
    {
        public override void OnGUI(Rect position)
        {
            position.yMin += EditorGUIUtility.singleLineHeight * 0.5f;
            position = EditorGUI.IndentedRect(position);
            GUI.Label(position, (attribute as L10nHeaderAttribute).header, EditorStyles.boldLabel);
        }

        public override float GetHeight()
        {
            return EditorGUIUtility.singleLineHeight * 1.5f;
        }
    }
}
