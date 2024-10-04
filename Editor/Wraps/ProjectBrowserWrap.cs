using System;
using System.Reflection;
using UnityEditor;

namespace jp.lilxyzw.editortoolbox
{
    internal class ProjectBrowserWrap
    {
        public static readonly Type type = typeof(Editor).Assembly.GetType("UnityEditor.ProjectBrowser");
        private static readonly FieldInfo FI_s_LastInteractedProjectBrowser = type.GetField("s_LastInteractedProjectBrowser", BindingFlags.Static | BindingFlags.Public);
        private static readonly FieldInfo FI_m_LastFolders = type.GetField("m_LastFolders", BindingFlags.NonPublic | BindingFlags.Instance);

        public EditorWindow w;
        public ProjectBrowserWrap(object instance) => w = instance as EditorWindow;

        public static ProjectBrowserWrap s_LastInteractedProjectBrowser => new(FI_s_LastInteractedProjectBrowser.GetValue(null));
        public string[] m_LastFolders => FI_m_LastFolders.GetValue(w) as string[];

        public static implicit operator EditorWindow(ProjectBrowserWrap wrap) => wrap.w;
    }
}
