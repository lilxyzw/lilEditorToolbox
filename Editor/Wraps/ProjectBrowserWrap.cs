using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;

namespace jp.lilxyzw.editortoolbox
{
    internal class ProjectBrowserWrap
    {
        public static readonly Type type = typeof(Editor).Assembly.GetType("UnityEditor.ProjectBrowser");
        private static readonly FieldInfo FI_s_LastInteractedProjectBrowser = type.GetField("s_LastInteractedProjectBrowser", BindingFlags.Static | BindingFlags.Public);
        private static readonly FieldInfo FI_m_LastFolders = type.GetField("m_LastFolders", BindingFlags.NonPublic | BindingFlags.Instance);
        private static readonly MethodInfo MI_GetAllProjectBrowsers = type.GetMethod("GetAllProjectBrowsers", BindingFlags.Public | BindingFlags.Static);

        private static Type T_List_ProjectBrowser = typeof(List<>).MakeGenericType(type);
        private static readonly PropertyInfo PI_Count = T_List_ProjectBrowser.GetProperty("Count");
        private static readonly PropertyInfo PI_Item = T_List_ProjectBrowser.GetProperty("Item");

        public EditorWindow w;
        public ProjectBrowserWrap(object instance) => w = instance as EditorWindow;

        public static ProjectBrowserWrap s_LastInteractedProjectBrowser => new(FI_s_LastInteractedProjectBrowser.GetValue(null));
        public static List<ProjectBrowserWrap> GetAllProjectBrowsers()
        {
            var list = MI_GetAllProjectBrowsers.Invoke(null,null);
            var count = (int)PI_Count.GetValue(list, null);

            var res = new List<ProjectBrowserWrap>();
            for(int i = 0; i < count; i++)
            {
                res.Add(new ProjectBrowserWrap(PI_Item.GetValue(list, new object[]{i})));
            }
            return res;
        }

        public string[] m_LastFolders => FI_m_LastFolders.GetValue(w) as string[];
    }
}
