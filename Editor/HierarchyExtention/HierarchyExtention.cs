using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace jp.lilxyzw.editortoolbox
{
    internal class HierarchyExtention
    {
        private static readonly Color32 missingScriptIconColor = new Color32(255,0,255,255);
        private const int ICON_SIZE = 16;
        internal static Texture2D m_missingScriptIcon;
        internal static Texture2D MissingScriptIcon()
        {
            if(m_missingScriptIcon) return m_missingScriptIcon;
            return m_missingScriptIcon = GenerateTexture(missingScriptIconColor,ICON_SIZE,ICON_SIZE);
        }
        private static EditorToolboxSettings Settings => EditorToolboxSettings.instance;
        private static List<IHierarchyExtentionConponent> hierarchyExtentionConponents;

        [InitializeOnLoadMethod] private static void Initialize() => EditorApplication.hierarchyWindowItemOnGUI += OnGUI;

        internal static readonly Type[] types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetCustomAttributes(typeof(ExportsComponent), false))
            .SelectMany(export => ((ExportsComponent)export).Types).ToArray();

        internal static readonly string[] names = types.Select(t => t.FullName).ToArray();

        internal static IHierarchyExtentionConponent Resolve()
        {
            hierarchyExtentionConponents = new List<IHierarchyExtentionConponent>();
            hierarchyExtentionConponents.AddRange(types.Where(t => Settings.hierarchyComponents.Contains(t.FullName)).Select(t => (IHierarchyExtentionConponent)Activator.CreateInstance(t)).OrderBy(c => c.Priority));
            return null;
        }

        // GUI
        private static void OnGUI(int instanceID, Rect selectionRect)
        {
            var go = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
            if(!go) return;

            var rectOrigin = selectionRect;
            selectionRect.x = selectionRect.xMax;

            if(hierarchyExtentionConponents == null) Resolve();

            foreach(var c in hierarchyExtentionConponents) c.OnGUI(ref selectionRect, go, instanceID, rectOrigin);
        }

        private static Texture2D GenerateTexture(Color32 color, int width, int height)
        {
            var tex = new Texture2D(width,height,TextureFormat.RGBA32,false);
            tex.SetPixels32(Enumerable.Repeat(color,width*height).ToArray());
            tex.Apply();
            return tex;
        }
    }
}
