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

        internal static GUIContent G(string key) => G(key, null, "");
        internal static GUIContent G(string key, string tooltip) => G(key, null, tooltip);
        internal static GUIContent G(string key, Texture image) => G(key, image, "");

        internal static GUIContent G(string key, Texture image, string tooltip)
        {
            if(!instance.localizationAsset) Load();
            if(guicontents.TryGetValue(key, out var content)) return content;
            return guicontents[key] = new GUIContent(L(key), image, L(tooltip));
        }
    }

    internal class L10nHeaderAttribute : PropertyAttribute
    {
        public readonly string key;
        public GUIContent header => L10n.G(key);
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

    internal class L10nHelpBoxAttribute : PropertyAttribute
    {
        public readonly string key;
        public readonly MessageType type;
        public string text => L10n.L(key);
        public GUIContent content => L10n.G(text, type == MessageType.None ? null : EditorGUIUtility.IconContent("console.infoicon").image);
        public L10nHelpBoxAttribute(string key, MessageType type = MessageType.None)
        {
            this.key = key;
            this.type = type;
        }
    }

    [CustomPropertyDrawer(typeof(L10nHelpBoxAttribute))]
    internal class L10nHelpBoxDrawer : DecoratorDrawer
    {
        public override void OnGUI(Rect position)
        {
            var attr = attribute as L10nHelpBoxAttribute;
            position = EditorGUI.IndentedRect(position);
            EditorGUI.HelpBox(position, attr.text, attr.type);
        }

        public override float GetHeight()
        {
            var attr = attribute as L10nHelpBoxAttribute;
            return EditorStyles.helpBox.CalcHeight(attr.content, EditorGUIUtility.currentViewWidth - 10);
        }
    }
}
