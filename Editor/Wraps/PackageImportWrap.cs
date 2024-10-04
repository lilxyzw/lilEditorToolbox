using System;
using System.Linq;
using System.Reflection;
using UnityEditor;

namespace jp.lilxyzw.editortoolbox
{
    internal class PackageImportWrap
    {
        public static readonly Type type = typeof(Editor).Assembly.GetType("UnityEditor.PackageImport");
        private static readonly MethodInfo MI_HasOpenInstances = typeof(EditorWindow).GetMethod("HasOpenInstances", BindingFlags.Static | BindingFlags.Public);
        private static readonly MethodInfo MI_HasOpenInstancesPackageImport = MI_HasOpenInstances.MakeGenericMethod(type);
        private static readonly FieldInfo FI_m_ImportPackageItems = type.GetField("m_ImportPackageItems", BindingFlags.NonPublic | BindingFlags.Instance);
        private static readonly MethodInfo MI_ShowTreeGUI = type.GetMethod("ShowTreeGUI", BindingFlags.NonPublic | BindingFlags.Instance);

        public EditorWindow w;
        public PackageImportWrap(object instance) => w = instance as EditorWindow;
        public static bool HasOpenInstances() => (bool)MI_HasOpenInstancesPackageImport.Invoke(null,null);
        public object[] m_ImportPackageItems => FI_m_ImportPackageItems.GetValue(w) as object[];
        public bool ShowTreeGUI(object[] items) => (bool)MI_ShowTreeGUI.Invoke(w, new object[]{items});
    }
}
