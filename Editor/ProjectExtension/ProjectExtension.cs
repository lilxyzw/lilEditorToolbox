using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace jp.lilxyzw.editortoolbox
{
    internal class ProjectExtension
    {
        internal static readonly Type[] types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetCustomAttributes(typeof(ExportsProjectExtensionComponent), false))
            .SelectMany(export => ((ExportsProjectExtensionComponent)export).Types).ToArray();

        internal static readonly string[] names = types.Select(t => t.FullName).ToArray();

        private static List<IProjectExtensionComponent> projectExtensionComponents;

        internal static IProjectExtensionComponent Resolve()
        {
            projectExtensionComponents = new List<IProjectExtensionComponent>();
            projectExtensionComponents.AddRange(types.Where(t => EditorToolboxSettings.instance.projectComponents.Contains(t.FullName)).Select(t => (IProjectExtensionComponent)Activator.CreateInstance(t)).OrderBy(c => c.Priority));
            return null;
        }

        [InitializeOnLoadMethod]
        private static void Init()
        {
            EditorApplication.projectWindowItemOnGUI -= Draw;
            EditorApplication.projectWindowItemOnGUI += Draw;
        }

        private static void Draw(string guid, Rect position)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            if(position.height > 16 || string.IsNullOrEmpty(path)) return;

            var name = Path.GetFileNameWithoutExtension(path);
            var extension = Path.GetExtension(path);
            var currentRect = position;
            currentRect.xMin += EditorStyles.label.CalcSize(new GUIContent(name)).x + 17;

            if(projectExtensionComponents == null) Resolve();

            foreach(var c in projectExtensionComponents) c.OnGUI(ref currentRect, guid, path, name, extension, position);
        }
    }
}
